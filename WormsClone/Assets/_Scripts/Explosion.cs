using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	[HideInInspector] public ExplosionDataHolder data;

	private void Start()
	{
		StartCoroutine(PlayAnimation());
	}

	IEnumerator PlayAnimation()
	{
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		for (int i = 0; i < data.animationFrames.Length; i++)
		{
			sRenderer.sprite = data.animationFrames[i];
			yield return new WaitForSeconds(Time.deltaTime*(1/Time.timeScale));
		}
		Destroy(gameObject);
	}
}
