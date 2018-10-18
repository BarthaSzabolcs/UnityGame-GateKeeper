using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomBehaviour : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] Collider2D attackTrigger;
    [SerializeField] float moveSpeed;
    [SerializeField] string[] taggedToAttack;

    Rigidbody2D self;
    Animator animator;

    bool move = true;
    #endregion

    private void Start()
    {
        self = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(move == true)
        {
            self.velocity = Vector2.left * moveSpeed;
        }
        animator.SetFloat("speed", Mathf.Abs(self.velocity.x));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        foreach (var tag in taggedToAttack)
        {
            if (tag == col.tag)
            {
                animator.SetBool("attack", true);
                self.velocity = Vector2.zero;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        foreach (var tag in taggedToAttack)
        {
            if (tag == col.tag)
            {
                animator.SetBool("attack", false);
                move = true;
            }
        }
    }
}
