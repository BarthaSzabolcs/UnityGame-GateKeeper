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
    void Start ()
	{
        var renderer = GetComponent<SpriteRenderer>();
        renderer.color = data.bulletColor;
        renderer.sprite = data.sprite;
        self = GetComponent<Rigidbody2D>();
		self.velocity = transform.up * data.speed;
		self.mass = data.mass;
        if(data.trail != null)
        {
            Instantiate(data.trail, transform);
        }
        if (data.hasLifeTime)
        {
            SelfDestruct component = gameObject.AddComponent<SelfDestruct>();
            component.lifeTime = data.lifeTime;
        }
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		foreach (var tag in data.taggedToDamage)
		{
			if (tag == coll.gameObject.tag)
			{
				coll.gameObject.GetComponent<Health>().TakeDamage(data.damage, gameObject);
			}
		}
		foreach (var tag in data.taggedToDestroy)
		{
			if (tag == coll.gameObject.tag)
			{
				if(data.impactAnim != null)
                {
                    Instantiate(data.impactAnim, transform.position, Quaternion.identity);
                }
                AudioManager.Instance.PlaySound(data.impactAudio);
				Destroy(gameObject);
			}
		}
		foreach (var tag in data.taggedToDestroyWithoutEffects)
		{
			if (tag == coll.gameObject.tag)
			{
				Destroy(gameObject);
			}
		}
	}
    #endregion
}
