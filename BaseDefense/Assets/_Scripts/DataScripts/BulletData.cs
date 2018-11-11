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
    public int damage;
    public int baseLayer;

    [Header("Penetration Settings:")]
    public string[] taggedToPenetrate;
    public int penetrationNumber;
    public float penetrationTime;
    public int penetrationLayer;

    [Header("Movement Settings:")]
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
    public GameObject trail;
    public string bulletFiredAudio;
    public string impactAudio;
    public ExplosionData explosionData;

    [Header("Homing Settings:")]
    public bool isHoming;
    public LayerMask targetMask;
    public string[] taggedToTarget;
    public float targetingRadius;
    public float homingRefreshRate;
    public float targetingRefreshRate;
    public float maxRotationPerCycle;

    [Header("AoE Settings:")]
    public bool isAoE;
    public LayerMask aoeLayerMask;
    public float aoeRadius;

    [Header("SmallBullet Settings:")]
    public BulletData smallerBullet;
    public int smallBulletCount;
    public float smallBulletAngle;
    public float smallBulletAngleOffset;
    public Vector2 smalBulletpositionOffSet;
}