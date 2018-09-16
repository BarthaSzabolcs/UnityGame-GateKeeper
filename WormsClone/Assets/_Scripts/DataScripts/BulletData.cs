using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Bullet")]
public class BulletData : ScriptableObject
{
	[Header("Interaction Settings:")]
	public string[] taggedToDamage;
	public string[] taggedToDestroy;
	public string[] taggedToDestroyWithoutEffects;
    public float mass;
    public int damage;
    public float speed;
    [Header("LifeTime Settings:")]
    public bool hasLifeTime;
    public float lifeTime;
    [Header("Effects Settings:")]
    public Sprite sprite;
    public Color bulletColor;
	public Color muzzleFlashColor;
	public float muzzleFlashSize;
    public GameObject trail;
	public string bulletFiredAudio;
    public string impactAudio;
	public GameObject impactAnim;
}
