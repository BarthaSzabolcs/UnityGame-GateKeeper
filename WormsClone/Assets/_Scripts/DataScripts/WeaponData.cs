using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    public Sprite sprite;
    public bool isAuto;
    public abstract void Attack();
}
