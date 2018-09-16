﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingPattern : ScriptableObject
{
    public abstract void Shoot(GameObject bullet, Transform self);
}
