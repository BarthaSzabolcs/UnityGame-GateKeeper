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

    private bool alive;
    private Coroutine penetrationCoroutine;
    private int penetrationCounter = 0;
    
    #endregion
    #region UnityFunctions

    public void Initialize()
    {
        alive = true;

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
        trailerRenderer.enabled = true;
        trailerRenderer.Clear();
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
            homingBehaviour.searchDelay = data.searchingDelay;
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
    void OnCollisionEnter2D(Collision2D col)
    {
        if(alive == false)
        {
            return;
        }

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
                    Explode();
                    return;
                }
            }
            foreach (string tag in data.taggedToPenetrate)
            {
                if (tag == col.gameObject.tag)
                {
                    if(penetrationCounter < data.penetrationsBeforeExplode && penetrationCoroutine == null)
                    {
                        penetrationCoroutine = StartCoroutine(PenetrationRoutine(-col.relativeVelocity));
                        return;
                    }
                    else
                    {
                        Explode();
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
            Explode();
            return;
        }
    }
    #endregion
    private void Explode()
    {
        alive = false;

        AudioManager.Instance.PlaySound(data.impactAudio);

        if (data.explosionData != null)
        {
            GameObject explosionInstance = ObjectPool.Instance.Spawn(ObjectPool.Types.explosion, transform.position);
            var explosionComponent = explosionInstance.GetComponent<Explosion>();
            explosionComponent.data = data.explosionData;
            explosionComponent.Initialize();
        }

        trailerRenderer.Clear();
        trailerRenderer.enabled = false;

        StopAllCoroutines();
        ObjectPool.Instance.Pool(ObjectPool.Types.bullet, gameObject);
    }
    private IEnumerator PenetrationRoutine(Vector2 velocity)
    {
        gameObject.layer = data.ghostLayer;
        self.velocity = velocity;

        yield return new WaitForSeconds(data.ghostTime);

        gameObject.layer = data.baseLayer;
        penetrationCounter++;

        penetrationCoroutine = null;
    }

}