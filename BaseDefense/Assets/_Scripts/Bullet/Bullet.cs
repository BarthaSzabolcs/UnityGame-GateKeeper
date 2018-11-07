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
    #endregion

    #region UnityFunctions
    public void Initialize()
    {
        gameObject.layer = data.layer;

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
            homingBehaviour.taggedToTarget = data.taggedToTarget;
            homingBehaviour.targetingRadius = data.targetingRadius;
            homingBehaviour.homingRefreshRate = data.homingRefreshRate;
            homingBehaviour.speed = data.speed;
            homingBehaviour.maxRotationPerCycle = data.maxRotationPerCycle;
        }
        else if(homingBehaviour != null)
        {
            Destroy(homingBehaviour);
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (data.isAoE == false)
        {
            foreach (var tag in data.taggedToDamage)
            {
                if (tag == coll.gameObject.tag)
                {
                    coll.gameObject.GetComponent<Health>().TakeDamage(data.damage);
                }
            }
            foreach (var tag in data.taggedToDestroy)
            {
                if (tag == coll.gameObject.tag)
                {
                    Explode();
                    break;
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
            Explode();
        }
    }
    #endregion
    void Explode()
    {
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
}