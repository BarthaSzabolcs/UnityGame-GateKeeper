using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Events
    public delegate void JetPackTriggered();
    public event JetPackTriggered OnJetPackTriggered;
    #endregion
    #region ShowInEditor
    [SerializeField] PlayerControlData data;
    [SerializeField] Transform feet;
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
    }
    #endregion

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
        if (Physics2D.OverlapCircle(feet.position, data.feetRadius, data.feetLayer))
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
            if (self.velocity.x - currentMoveForce > -currentMaxSpeed)
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
            if (self.velocity.x + currentMoveForce < currentMaxSpeed)
            {
                self.AddForce(Vector2.right * currentMoveForce);
            }
            else
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
                self.AddForce(Vector2.up * data.jumpForce);
                jumpCounter = 0;
            }
            else if (jumpCounter < data.multiJumps)
            {
                self.AddForce(Vector2.up * data.jumpForce);
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

}
