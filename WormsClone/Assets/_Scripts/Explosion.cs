using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region HideInEditor
    [HideInInspector] public ExplosionData data;
    SpriteRenderer sRenderer;
    #endregion

    private void Start()
	{
		StartCoroutine(PlayAnimation());
	}
	IEnumerator PlayAnimation()
	{
        if (data.isExplosive)
        {
            var circleCollider = gameObject.AddComponent<CircleCollider2D>();
            circleCollider.radius = data.explosionRange;
            circleCollider.isTrigger = true;
            circleCollider.usedByEffector = true;

            var pointEffector = gameObject.AddComponent<PointEffector2D>();
            pointEffector.useColliderMask = true;
            pointEffector.colliderMask = data.explosionMask;
            pointEffector.forceMagnitude = data.explosionMagnitude;
            pointEffector.forceSource = EffectorSelection2D.Collider;
            pointEffector.forceMode = data.explosionForceMode;
        }
        sRenderer = GetComponent<SpriteRenderer>();
		for (int i = 0; i < data.animationFrames.Length; i++)
		{
			sRenderer.sprite = data.animationFrames[i];
            yield return new WaitForEndOfFrame();
		}
		Destroy(gameObject);
	}
}
