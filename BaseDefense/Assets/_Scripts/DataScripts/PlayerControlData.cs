using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerControl")]
public class PlayerControlData : ScriptableObject
{
    public PhysicsMaterial2D material;
    [Header("Animation Settings:")]
    public Vector2 weaponPosition;

    [Header("Grip Settings:")]
    public float gripWidth;
    public LayerMask gripLayer;

    [Header("Movement Settings:")]
    public float moveForce;
    public float runMaxSpeed;
    public float sprintMaxSpeed;
    public float airControlMultiplier;

    [Header("Jump Settings:")]
    public float jumpForce;
    public int multiJumps;

    [Header("Teleport Settings")]
    public float teleportRange;
    public float teleportCoolDown;
    public LayerMask teleportMask;
    public bool loseMomentumOnTeleport;
}
