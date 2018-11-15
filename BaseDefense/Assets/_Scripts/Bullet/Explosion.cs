using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region Show In Editor

    [Header("Components:")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] CircleCollider2D explosionCollider;
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
			explosionCollider.enabled = true;
	        explosionCollider.radius = data.explosionRange;
            
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
			explosionCollider.enabled = false;

            // Effector
            pointEffector.enabled = false;
		}

        data.animationCllection.Randomize(spriteRenderer);

        // Play Animation
        foreach (var sprite in data.animationCllection.Next())
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.01667f);
        }

        // Object Pooling
		spriteRenderer.sprite = null;
		ObjectPool.Instance.Pool(ObjectPool.Types.explosion, gameObject);
	}
}
