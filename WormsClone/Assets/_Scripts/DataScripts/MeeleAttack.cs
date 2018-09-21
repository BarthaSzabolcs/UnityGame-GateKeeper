using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeeleAttack : ScriptableObject
{
    public int positiveLoops, negativeLoops;

    public abstract void PositivePhase(Rigidbody2D self, bool isLookingRight);
    public abstract void NegativePhase(Rigidbody2D self, bool isLookingRight);
}
