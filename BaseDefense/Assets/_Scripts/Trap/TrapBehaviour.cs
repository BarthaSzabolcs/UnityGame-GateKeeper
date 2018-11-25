using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapBehaviour : ScriptableObject
{
    [Header("Trap")]
    public bool enableTrigger;

    public abstract void Initialize(Transform transform, Collider2D triggerCollider);
    public abstract void CleanUp();
}
