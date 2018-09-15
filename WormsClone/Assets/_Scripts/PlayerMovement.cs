using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Events
    public delegate void JetPackTriggered();
    public event JetPackTriggered OnJetPackTriggered;
    #endregion
    #region ShowInEditor
    [Header("Movement:")]
    [SerializeField] float moveForce;
    [SerializeField] float maxSpeed;

    [Header("Jump:")]
    [SerializeField] float jumpForce;
    [SerializeField] int multiJumps;
    [SerializeField] Transform feet;
    [SerializeField] LayerMask jumpLayer;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    int jumpCounter = 0;
    #endregion

    #region Magic
    void Start ()
    {
        self = GetComponent<Rigidbody2D>();
	}
	void Update ()
    {
        Move();
    }
    #endregion

    void Move()
    {
        HorizontalMovement();
        Jump();
        JetPack();
        Teleport();
    }

    void HorizontalMovement()
    {
        if (Input.GetButton("Left"))
        {
            self.AddForce(Vector2.left * moveForce);
        }
        if (Input.GetButton("Right"))
        {
            self.AddForce(Vector2.right * moveForce);
        }

        if (Mathf.Abs(self.velocity.x) > maxSpeed)
        {
            self.velocity = new Vector2(Mathf.Sign(self.velocity.x) * maxSpeed, self.velocity.y);
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (Grounded())
            {
                self.AddForce(Vector2.up * jumpForce);
                jumpCounter = 0;
            }
            else if (jumpCounter < multiJumps)
            {
                self.AddForce(Vector2.up * jumpForce);
                jumpCounter++;
            }
        }
    }
    bool Grounded()
    {
        if (Physics2D.OverlapCircle(feet.position, 0.125f, jumpLayer))
        {
            return true;
        }
        else
        {
            return false;
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
            Vector2 colliderSize = GetComponent<BoxCollider2D>().size;
            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 pointA = new Vector2(-colliderSize.x / 2, -colliderSize.y / 2) + target;
            Vector2 pointB = new Vector2(colliderSize.x / 2, colliderSize.y / 2) + target;

            if (Physics2D.OverlapArea(pointA, pointB) == null) 
            {
                self.velocity = Vector2.zero;
                transform.position = target;
            }
        }
    }
}
