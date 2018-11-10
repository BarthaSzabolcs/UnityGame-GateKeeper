using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region ShowInEditor
    public BulletData data;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    private int penetrationCounter = 0;
    #endregion

    #region UnityFunctions
    public void Initialize()
    {
        // Reset Counters
        penetrationCounter = 0;

        gameObject.layer = data.baseLayer;

        var renderer = GetComponent<SpriteRenderer>();
        renderer.color = data.bulletColor;
        renderer.sprite = data.sprite;

        self = GetComponent<Rigidbody2D>();
        self.velocity = transform.up * data.speed;
        self.gravityScale = data.gravityScale;
        self.mass = data.mass;

        // Initialize Trail
        GameObject trail = null; 
        if(transform.childCount > 0)
        {
            trail = transform.GetChild(0).gameObject;
        }
        if (trail == null)
        {
            if (data.trail != null)
            {
                Instantiate(data.trail, transform);
            }
        }
        else
        {
            Destroy(trail);
            if (data.trail != null)
            {
                Instantiate(data.trail, transform);
            }
        }

        // Initialize SelfDestruct
        var selfDestruct = GetComponent<SelfDestruct>();
        selfDestruct.StopSelfDestruct();
        if (data.hasLifeTime)
        {
            selfDestruct.lifeTime = data.lifeTime;
            if (data.explodeAfterLifeTime)
            {
                selfDestruct.explosionAudio = data.impactAudio;
                selfDestruct.explosionData = data.explosionData;
            }
            selfDestruct.StartSelfDestruct();
        }

        // Initialize HomingBehavior
        var homingBehaviour = GetComponent<HomingBehavior>();
        if (data.isHoming)
        {
            if (homingBehaviour == null)
            {
                homingBehaviour = gameObject.AddComponent<HomingBehavior>();
            }
            homingBehaviour.targetMask = data.targetMask;
            homingBehaviour.taggedToTarget = data.taggedToTarget;
            homingBehaviour.targetingRadius = data.targetingRadius;
            homingBehaviour.targetingRefreshRate = data.targetingRefreshRate;
            homingBehaviour.homingRefreshRate = data.homingRefreshRate;
            homingBehaviour.speed = data.speed;
            homingBehaviour.maxRotationPerCycle = data.maxRotationPerCycle;
            homingBehaviour.Initialize();
        }
        else if(homingBehaviour != null)
        {
            Destroy(homingBehaviour);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (data.isAoE == false)
        {
            foreach (var tag in data.taggedToDamage)
            {
                if (tag == col.gameObject.tag)
                {
                    col.gameObject.GetComponent<Health>().TakeDamage(data.damage);
                    break;
                }
            }
            foreach (var tag in data.taggedToDestroy)
            {
                if (tag == col.gameObject.tag)
                {
                    Explode(/*col.contacts[0].point*/);
                    return;
                }
            }
            foreach (string tag in data.taggedToPenetrate)
            {
                if (tag == col.gameObject.tag)
                {
                    if(penetrationCounter < data.penetrationNumber)
                    {
                        Penetrate();
                    }
                    else
                    {
                        Explode(/*col.contacts[0].point*/);
                        return;
                    }
                }
            }
        }
        else
        {
            Collider2D[] objectsHit = Physics2D.OverlapCircleAll(transform.position, data.aoeRadius, data.aoeLayerMask);
            foreach (var hit in objectsHit)
            {
                foreach (var tag in data.taggedToDamage)
                {
                    if (tag == hit.gameObject.tag)
                    {
                        hit.gameObject.GetComponent<Health>().TakeDamage(data.damage);
                    }
                }
            }
            Explode(/*col.contacts[0].point*/);
            return;
        }
    }
    #endregion
    private void Explode(/*Vector2 contactPoint*/)
    {
        //DeathSpawn(contactPoint);
        AudioManager.Instance.PlaySound(data.impactAudio);
        if (data.explosionData != null)
        {
			GameObject explosionInstance = ObjectPool_Manager.Instance.SpawnExplosion(transform.position);
			var explosionComponent = explosionInstance.GetComponent<Explosion>();
			explosionComponent.data = data.explosionData;
			explosionComponent.Initialize();
		}
        ObjectPool_Manager.Instance.PoolBullet(gameObject);
    }
    private void Penetrate()
    {
        StartCoroutine(PenetrationRoutine());
    }
    private IEnumerator PenetrationRoutine()
    {
        penetrationCounter++;

        gameObject.layer = data.penetrationLayer;
        yield return new WaitForSeconds(data.penetrationTime);
        gameObject.layer = data.baseLayer;
    }
    private void DeathSpawn(Vector2 contactPoint)
    {
        if(data.smallerBullet != null)
        {
            int splits = data.smallBulletCount - 1;
            GameObject bulletInstance;
            Bullet bulletComponent;

            Vector2 direction = contactPoint - (Vector2)transform.position;
            direction.Normalize();

            Vector2 spawnPosition = transform.position + transform.TransformVector(data.smalBulletpositionOffSet);//(Vector2)transform.position + direction * data.smalBulletpositionOffSet;
            float impactAngle = Vector2.SignedAngle(transform.up, direction);

            if (splits != 0)
            {
                for (var i = 0; i < splits; i++)
                {
                    bulletInstance = ObjectPool_Manager.Instance.SpawnBullet(spawnPosition);
                    bulletComponent = bulletInstance.GetComponent<Bullet>();

                    bulletInstance.transform.Rotate
                    (
                        0,
                        0,
                        i * data.smallBulletAngle / splits - data.smallBulletAngle * 0.5f + impactAngle + data.smallBulletAngleOffset
                    );

                    bulletComponent.data = data.smallerBullet;
                    bulletComponent.Initialize();
                }
            }
            else
            {
                bulletInstance = ObjectPool_Manager.Instance.SpawnBullet(spawnPosition);
                bulletComponent = bulletInstance.GetComponent<Bullet>();

                bulletInstance.transform.Rotate(0, 0, impactAngle + data.smallBulletAngleOffset);
                bulletComponent.data = data.smallerBullet;
                bulletComponent.Initialize();
            }
        }
    }
}