using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	[HideInInspector] public ExplosionData data;
    SpriteRenderer sRenderer;
    private void Start()
	{
		StartCoroutine(PlayAnimation());
	}

	IEnumerator PlayAnimation()
	{
        sRenderer = GetComponent<SpriteRenderer>();
		for (int i = 0; i < data.animationFrames.Length; i++)
		{
			sRenderer.sprite = data.animationFrames[i];
            yield return new WaitForEndOfFrame();
		}
		Destroy(gameObject);
	}
}
