using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] PlayerControlData data;
    [SerializeField] Transform grip;
    [SerializeField] Transform weaponTransform;
    [SerializeField] JetPack jetPack;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    SpriteRenderer sRenderer;
    Weapon weapon;
    bool isGrounded;
    int jumpCounter = 0;
    float currentMaxSpeed;
    float currentMoveForce;
    float teleportTimer;
    Vector2 target;
    #endregion

    #region UnityFunctions

    void Awake ()
    {
        self = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        weapon = weaponTransform.GetComponent<Weapon>();
    }
	void Update ()
    {
        if (GameManager.Instance.InBuildMode == false)
        {
            WeaponHandling();
            Move();
        }
        OtherInput();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "DroppedWeapon")
        {
            PickUpWeapon(col.gameObject.GetComponent<DroppedWeapon>().data);
            Destroy(col.gameObject);
        }
    }

    #endregion
    #region Movement Functions

    void Move()
    {
        Grounded();
        AltVelocity();
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
            jumpCounter = 0;
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
                self.AddForce(Vector2.left * currentMoveForce);
            }
            else
            {
                self.velocity = new Vector2(-currentMaxSpeed, self.velocity.y);
            }
        }
        else if (Input.GetButton("Right"))
        {
            if (self.velocity.x < currentMaxSpeed)
            {
                self.AddForce(Vector2.right * currentMoveForce);
            }
            else
            {
                self.velocity = new Vector2(currentMaxSpeed, self.velocity.y);
            }
        }
        else if(isGrounded == true)
        {
            self.velocity = new Vector2( Mathf.Lerp(self.velocity.x, 0f, data.stoppingRate) , self.velocity.y);
        }
    }
    void AltVelocity()
    {
        if (Input.GetButton("AltVelocity") && isGrounded)
        {
            currentMaxSpeed = data.altMaxSpeed;
        }
        else
        {
            currentMaxSpeed = data.baseMaxSpeed;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                self.velocity = new Vector2(self.velocity.x, data.jumpForce);
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
            jetPack.Use();
        }
    }
    void Teleport()
    {
        if (Input.GetButtonDown("Teleport"))
        {
            if (Vector2.Distance(target, transform.position) < data.teleportRange && teleportTimer + data.teleportCoolDown < Time.time)
            {
                Vector2 colliderSize = GetComponent<Collider2D>().bounds.size;

                Vector2 pointA = new Vector2(-colliderSize.x / 2, -colliderSize.y / 2) + target;
                Vector2 pointB = new Vector2(colliderSize.x / 2, colliderSize.y / 2) + target;


                if (Physics2D.OverlapArea(pointA, pointB, data.teleportMask) == null)
                {
                    transform.position = target;
                    teleportTimer = Time.time;
                    if(data.loseVelocityOnTeleport)
                    {
                        self.velocity = Vector2.zero;
                    }
                }                
            }
        }
    }

    #endregion
    #region Weapon Functions

    void WeaponHandling()
    {
        AimWeapon();
        CheckDirection();
        Attack();
        Reload();
        ChangeWeapon();
        DropWeapon();
    }
    void AimWeapon()
    {
        // Set target
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Rotate the weapon towards the target based on the difference in angle
        weaponTransform.Rotate(new Vector3(0, 0, Vector2.SignedAngle(weaponTransform.right, target - (Vector2)weaponTransform.position)));
    }
    void Attack()
    {
        if (Input.GetButton("Attack"))
        {
            weapon.PullTrigger();
        }
        if (Input.GetButtonUp("Attack"))
        {
            weapon.RelaseTrigger();
        }
    }
    void Reload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            weapon.Reload();
        }
    }

    void CheckDirection()
    {
        if (target.x < transform.position.x)
        {
            sRenderer.flipX = true;
            weapon.SetApearence(true);
        }
        else
        {
            sRenderer.flipX = false;
            weapon.SetApearence(false);
        }
    }

    void ChangeWeapon()
    {
        if (Input.GetButtonDown("NextWeapon"))
        {
            weapon.ChangeWeapon(true);
        }
        if (Input.GetButtonDown("PreviousWeapon"))
        {
            weapon.ChangeWeapon(false);
        }
    }
    void DropWeapon()
    {
        if (Input.GetButtonDown("DropWeapon"))
        {
            weapon.DropWeapon();
        }
    }
    void PickUpWeapon(WeaponData weaponData)
    {
        weapon.AddWeapon(weaponData);
    }
    #endregion
    #region Other Functions
    void OtherInput()
    {
        BuildMode();
    }
    void BuildMode()
    {
        if (Input.GetButtonDown("BuildMode"))
        {
            GameManager.Instance.InBuildMode = true;
        }
        else if (Input.GetButtonUp("BuildMode"))
        {
            GameManager.Instance.InBuildMode = false;
        }
    }
    #endregion
}
