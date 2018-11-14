using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerControl")]
public class PlayerControlData : ScriptableObject
{
    [Header("Grip Settings:")]
    public float gripWidth;
    public LayerMask gripLayer;

    [Header("Movement Settings:")]
    public float moveForce;
    public float baseMaxSpeed;
    public float altMaxSpeed;
    public float airControlMultiplier;
    public float stoppingRate;

    [Header("Jump Settings:")]
    public float jumpForce;
    public int multiJumps;

    [Header("Teleport Settings")]
    public float teleportRange;
    public float teleportCoolDown;
    public LayerMask teleportMask;
    public bool loseVelocityOnTeleport;
}
