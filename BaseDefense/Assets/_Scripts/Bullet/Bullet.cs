using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region ShowInEditor
    [Header("Settings:")]
    public BulletData data;

    [Header("Components:")]
    [SerializeField] Rigidbody2D self;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] TrailRenderer trailerRenderer;
    #endregion
    #region HideInEditor
    private int penetrationCounter = 0;
    #endregion

    #region UnityFunctions
    public void Initialize()
    {
        // Layer
        gameObject.layer = data.baseLayer;

        // Penetration Counters
        penetrationCounter = 0;

        // RigidBody2D
        self.velocity = transform.up * data.speed;
        self.gravityScale = data.gravityScale;
        self.mass = data.mass;

        // SpriteRenderer
        renderer.color = data.bulletColor;
        renderer.sprite = data.sprite;

        // Trail        
        trailerRenderer.colorGradient = data.trailColor;
        trailerRenderer.widthCurve = data.trailWidth;
        trailerRenderer.widthMultiplier = data.trailWidthMultiplier;
        trailerRenderer.time = data.trailTime;

        // SelfDestruct
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

        // HomingBehavior
        var homingBehaviour = GetComponent<HomingBehavior>();
        if (data.isHoming)
        {
            homingBehaviour.enabled = true;
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
            homingBehaviour.enabled = false;
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
                    if(penetrationCounter < data.penetrationsBeforeExplode)
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
        //if (data.spawnBullet == true)
        //{
        //    DeathSpawn(contactPoint);
        //}
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

        gameObject.layer = data.ghostLayer;
        yield return new WaitForSeconds(data.ghostTime);
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