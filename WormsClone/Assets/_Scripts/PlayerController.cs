using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Events
    public delegate void JetPackTrigger();
    public event JetPackTrigger OnJetPackTriggered;

    public delegate void AttackTrigger();
    public event AttackTrigger OnAttackTriggered;

    public delegate void ReloadTrigger();
    public event ReloadTrigger OnReloadTriggered;
    #endregion
    #region ShowInEditor
    [SerializeField] PlayerControlData data;
    [SerializeField] Transform grip;
    [SerializeField] Transform weapon;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    bool isGrounded;
    float currentMaxSpeed;
    float currentMoveForce;
    int jumpCounter = 0;
    float teleportTimer;
    #endregion

    #region UnityFunctions
    void Start ()
    {
        self = GetComponent<Rigidbody2D>();
        self.sharedMaterial = data.material;
    }
	void Update ()
    {
        Move();
        WeaponHandling();
    }
    #endregion
    #region Movement Functions
    void Move()
    {
        Grounded();
        Sprint();
        HorizontalMovement();
        Jump();
        JetPack();
        Teleport();
    }

    void Grounded()
    {
        Vector2 pointA = new Vector2(grip.position.x - data.gripWidth, grip.position.y - data.gripWidth);
        Vector2 pointB = new Vector2(grip.position.x + data.gripWidth, grip.position.y + data.gripWidth);

        if (Physics2D.OverlapArea(pointA, pointB ,data.gripLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void HorizontalMovement()
    {
        if (isGrounded)
        {
            currentMoveForce = data.moveForce;
        }
        else
        {
            currentMoveForce = data.moveForce * data.airControlMultiplier;
        }

        if (Input.GetButton("Left"))
        {
            if (self.velocity.x > -currentMaxSpeed)
            {
                self.velocity = new Vector2(-currentMaxSpeed, self.velocity.y);
            }
        }
        else if (Input.GetButton("Right"))
        {
            if (self.velocity.x < currentMaxSpeed)
            {
                self.velocity = new Vector2(currentMaxSpeed, self.velocity.y);  
            }
        }
    }
    void Sprint()
    {
        if (Input.GetButton("Sprint") && isGrounded)
        {
            currentMaxSpeed = data.sprintMaxSpeed;
        }
        else
        {
            currentMaxSpeed = data.runMaxSpeed;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                self.velocity = new Vector2(self.velocity.x, data.jumpForce);
                jumpCounter = 0;
            }
            else if (jumpCounter < data.multiJumps)
            {
                self.velocity = new Vector2 (self.velocity.x, data.jumpForce);
                jumpCounter++;
            }
        }
    }
    void JetPack()
    {
        if (Input.GetButton("JumpJet"))
        {
            if (OnJetPackTriggered != null)
            {
                OnJetPackTriggered();
            }
        }
    }
    void Teleport()
    {
        if (Input.GetButtonDown("Teleport"))
        {

            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(target, transform.position) < data.teleportRange && teleportTimer + data.teleportCoolDown < Time.time)
            {
                Vector2 colliderSize = GetComponent<BoxCollider2D>().size;
                Vector2 pointA = new Vector2(-colliderSize.x / 2, -colliderSize.y / 2) + target;
                Vector2 pointB = new Vector2(colliderSize.x / 2, colliderSize.y / 2) + target;

                if (Physics2D.OverlapArea(pointA, pointB) == null)
                {
                    transform.position = target;
                }

                teleportTimer = Time.time;
            }
        }
    }
    #endregion
    #region Weapon Functions
    void WeaponHandling()
    {
        AimWeapon();
        Attack();
        Reload();
    }
    void AimWeapon()
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weapon.up = target - (Vector2)weapon.position;
    }
    void Attack()
    {
        if(Input.GetButton("Attack"))
        {
            if (OnAttackTriggered != null)
            {
                OnAttackTriggered();
            }
        }
    }
    void Reload()
    {
        if (Input.GetButton("Reload"))
        {
            if (OnReloadTriggered != null)
            {
                OnReloadTriggered();
            }
        }
    }
    #endregion
}
