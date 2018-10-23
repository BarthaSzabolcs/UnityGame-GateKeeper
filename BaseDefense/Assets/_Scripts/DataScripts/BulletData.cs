using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Bullet")]
public class BulletData : ScriptableObject
{
    [Header("Interaction Settings:")]
    public int layer;
    public string[] taggedToDamage;
    public string[] taggedToDestroy;
    public string[] taggedToDestroyWithoutEffects;
    public int damage;
    public float speed;
    public float mass;
    public float gravityScale;

    [Header("LifeTime Settings:")]
    public bool hasLifeTime;
    public float lifeTime;
    public bool explodeAfterLifeTime;

    [Header("Effects Settings:")]
    public Sprite sprite;
    public Color bulletColor;
    public Color muzzleFlashColor;
    public float muzzleFlashSize;
    public GameObject trail;
    public string bulletFiredAudio;
    public string impactAudio;
    public ExplosionData explosionData;
    public GameObject explosionObject;

    [Header("Homing Settings:")]
    public bool isHoming;
    public string[] taggedToTarget;
    public float targetingRadius;
    public float homingRefreshRate;
    public float maxRotationPerCycle;

    [Header("AoE Settings:")]
    public bool isAoE;
    public float aoeRadius;
}