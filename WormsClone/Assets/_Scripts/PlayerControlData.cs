using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/PlayerControlData")]
public class PlayerControlData : ScriptableObject
{
    public PhysicsMaterial2D material;
    [Header("Feet Settings:")]
    public float feetRadius;
    public LayerMask feetLayer;

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
}
