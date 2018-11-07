using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region HideInEditor
    [HideInInspector] public ExplosionData data;
    SpriteRenderer sRenderer;
    #endregion

    public void Initialize()
	{
		StartCoroutine(PlayAnimation());
	}
	IEnumerator PlayAnimation()
	{
		CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
		PointEffector2D pointEffector = GetComponent<PointEffector2D>();
		if (data.isExplosive)
        {
			if (circleCollider == null)
			{
				circleCollider = gameObject.AddComponent<CircleCollider2D>();
			}
			circleCollider.enabled = true;
	        circleCollider.radius = data.explosionRange;
            circleCollider.isTrigger = true;
            circleCollider.usedByEffector = true;

			if (pointEffector == null)
			{
				pointEffector = gameObject.AddComponent<PointEffector2D>();
			}
			pointEffector.enabled = true;
            pointEffector.useColliderMask = true;
            pointEffector.colliderMask = data.explosionMask;
            pointEffector.forceMagnitude = data.explosionMagnitude;
            pointEffector.forceSource = EffectorSelection2D.Collider;
            pointEffector.forceMode = data.explosionForceMode;
        }
		else
		{
			if(circleCollider != null)
			{
				circleCollider.enabled = false;
			}
			if (pointEffector != null)
			{
				pointEffector.enabled = false;
			}
		}
        sRenderer = GetComponent<SpriteRenderer>();
		for (int i = 0; i < data.animationFrames.Length; i++)
		{
			sRenderer.sprite = data.animationFrames[i];
            yield return new WaitForEndOfFrame();
		}
		sRenderer.sprite = null;
		ObjectPool_Manager.Instance.PoolExplosion(gameObject);
	}
}
