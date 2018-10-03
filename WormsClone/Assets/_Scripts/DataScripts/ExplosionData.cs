﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Explosion")]
public class ExplosionData : ScriptableObject
{
	public Sprite[] animationFrames;
    [Header("Effector Setting:")]
    public bool isExplosive;
    public float explosionRange;
    public float explosionMagnitude;
    public LayerMask explosionMask;
    public EffectorForceMode2D explosionForceMode;
}
