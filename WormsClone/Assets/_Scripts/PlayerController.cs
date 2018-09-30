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

    public delegate void WeaponChangeTrigger(bool next);
    public event WeaponChangeTrigger OnWeaponChangeTriggered;

    public delegate void WeaponDropTrigger();
    public event WeaponDropTrigger OnWeaponDropTriggered;
    #endregion
    #region ShowInEditor
    [SerializeField] PlayerControlData data;
    [SerializeField] Transform grip;
    [SerializeField] Transform weaponTransform;
    [SerializeField] Animator animator;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    SpriteRenderer sRenderer;
    Weapon weaponComponent;
    
    bool isGrounded;
    int jumpCounter = 0;

    float currentMaxSpeed;
    float currentMoveForce;

    float teleportTimer;

    [HideInInspector] public bool weaponIsAuto;
    [HideInInspector] public bool canAim = true;
    [HideInInspector] public Vector2 target;
    #endregion

    #region UnityFunctions
    void Start ()
    {
        weaponComponent = weaponTransform.GetComponent<Weapon>();
        self = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        self.sharedMaterial = data.material;
    }
	void Update ()
    {
        Move();
        WeaponHandling();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "DroppedWeapon")
        {
            DroppedWeapon droppedWeapon = col.gameObject.GetComponent<DroppedWeapon>();
            PickUpWeapon(droppedWeapon.data);
            Destroy(col.gameObject);
        }
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
        animator.SetFloat("speed", Mathf.Abs(self.velocity.x));
        animator.SetBool("isGrounded", isGrounded);
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
                    teleportTimer = Time.time;
                    if(data.loseMomentumOnTeleport)
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
        if (canAim)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            weaponTransform.right = target - (Vector2)weaponTransform.position;
        }
    }
    void Attack()
    {
        if (weaponIsAuto)
        {
            if (Input.GetButton("Attack"))
            {
                if (OnAttackTriggered != null) OnAttackTriggered();
            }
        }
        else
        {
            if (Input.GetButtonDown("Attack"))
            {
                if (OnAttackTriggered != null) OnAttackTriggered();
            }
        }
        
    }
    void Reload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            if (OnReloadTriggered != null) OnReloadTriggered();
        }
    }

    void CheckDirection()
    {
        if(target.x > transform.position.x)
        {
            sRenderer.flipX = false;
            weaponComponent.RefreshAppearance(true);
        }
        else
        {
            sRenderer.flipX = true;
            weaponComponent.RefreshAppearance(false);
        }
    }

    void ChangeWeapon()
    {
        if (Input.GetButtonDown("NextWeapon"))
        {
            if (OnWeaponChangeTriggered != null) OnWeaponChangeTriggered(true);
        }
        if (Input.GetButtonDown("PreviousWeapon"))
        {
            if (OnWeaponChangeTriggered != null) OnWeaponChangeTriggered(false);
        }
    }
    void DropWeapon()
    {
        if (Input.GetButtonDown("DropWeapon"))
        {
            if (OnWeaponDropTriggered != null) OnWeaponDropTriggered();
        }
    }
    void PickUpWeapon(WeaponData weaponData)
    {
        weaponComponent.PickUpWeapon(weaponData);
    }
    #endregion
}
