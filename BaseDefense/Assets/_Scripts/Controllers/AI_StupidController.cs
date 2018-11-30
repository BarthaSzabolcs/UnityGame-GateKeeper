using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StupidController : MonoBehaviour
{
    #region ShowInEditor

    [Header("Attack:")]
    [SerializeField] int damage;
    [SerializeField] float fallBackForce;
    [SerializeField] string[] taggedToDamge;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AI_Sight sight;
    [SerializeField] float moveForce;
    [SerializeField] float maxSpeed;
    [SerializeField] string targetName;

    #endregion
    #region HideInEditor

    Rigidbody2D self;
    float sign = 1;
    Transform target;
    
    #endregion

    private void Start()
    {
        self = GetComponent<Rigidbody2D>();
        target = GameObject.Find(targetName).transform;
        CheckDirection();
    }
    private void Update()
    {
        if (/*sight.TargetVisible == false && */Mathf.Abs(self.velocity.x) < maxSpeed)
        {
            self.AddForce(sign * Vector2.left * moveForce);
        }
        else /*if (sight.TargetVisible == false)*/
        {
            self.velocity = new Vector2(sign * -maxSpeed, self.velocity.y);
        }
    }
    private void CheckDirection()
    {
        sign = Mathf.Sign(transform.position.x - target.position.x);
        if (sign > 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        foreach (string tag in taggedToDamge)
        {
            if(col.gameObject.tag == tag)
            {
                col.gameObject.GetComponent<Health>().TakeDamage(damage);
                self.AddForce(Vector2.right * sign * fallBackForce);
            }
        }
    }
}