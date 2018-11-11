using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region Show In Editor

    [Header("Components:")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] CircleCollider2D explosiobCollider;
    [SerializeField] PointEffector2D pointEffector;
    
    #endregion
    #region HideInEditor

    [HideInInspector] public ExplosionData data;
    
    #endregion

    public void Initialize()
	{
        spriteRenderer.color = data.animationColor;
        
        // Play it
        StartCoroutine(PlayAnimation());
    }
    IEnumerator PlayAnimation()
	{
        // Initialize Explosion
		if (data.isExplosive)
        {
            // Collider
			explosiobCollider.enabled = true;
	        explosiobCollider.radius = data.explosionRange;
            
            // Effector
			pointEffector.enabled = true;
            pointEffector.useColliderMask = true;
            pointEffector.colliderMask = data.explosionMask;
            pointEffector.forceMagnitude = data.explosionMagnitude;
            pointEffector.forceSource = EffectorSelection2D.Collider;
            pointEffector.forceMode = data.explosionForceMode;
        }
		else
		{
            // Collider
			explosiobCollider.enabled = false;

            // Effector
            pointEffector.enabled = false;
		}

        data.animationCllection.Randomize(spriteRenderer);

        // Play Animation
        foreach (var sprite in data.animationCllection.Next())
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForEndOfFrame();
        }

        // Object Pooling
		spriteRenderer.sprite = null;
		ObjectPool_Manager.Instance.PoolExplosion(gameObject);
	}
}
