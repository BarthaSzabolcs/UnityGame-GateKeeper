using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    [Header("Appearance Settings:")]
    public Sprite sprite;
    public Vector2 weaponPosition;
    public Vector2 rightHandPosition;
    public Vector2 leftHandPosition;
    [Header("Control Settings:")]
    public bool isAuto;

    public abstract void Attack();
}
