using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Bullet")]
public class BulletData : ScriptableObject
{

    [Header("VisualFX:")]
    public Sprite sprite;
    public Color bulletColor;
    public Color muzzleFlashColor;

    [Header("   Trail:")]
    public AnimationCurve trailWidth;
    public float trailWidthMultiplier;
    public float trailTime;
    public Gradient trailColor;

    [Header("AudioFX:")]
    public string impactAudio;
    public ExplosionData explosionData;

    [Header("Movement:")]
    public float speed;
    public float mass;
    public float gravityScale;

    [Header("Interaction:")]
    public string[] taggedToDamage;
    public string[] taggedToDestroy;
    public string[] taggedToDestroyWithoutEffects;
    public int baseLayer;

    [Header("   Penetration:")]
    public string[] taggedToPenetrate;
    public int penetrationsBeforeExplode;
    public float ghostTime;
    public int ghostLayer;

    [Header("   Damage:")]
    public int damage;
    public bool isAoE;
    public LayerMask aoeLayerMask;
    public float aoeRadius;

    [Header("Additional Behaviour:")]
    public bool hasLifeTime;
    public bool isHoming;
    public bool spawnBullet;

    [Header("   LifeTime:")]
    public float lifeTime;
    public bool explodeAfterLifeTime;

    [Header("   Home in:")]
    public LayerMask targetMask;
    public string[] taggedToTarget;
    public float targetingRadius;
    public float searchingDelay;
    public float homingRefreshRate;
    public float targetingRefreshRate;
    public float maxRotationPerCycle;

}