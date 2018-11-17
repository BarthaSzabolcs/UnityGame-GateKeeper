using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    #region Show In Editor

    [Header("Components")]
    [SerializeField] Rigidbody2D self;

    [Header("Damage:")]
    [SerializeField] string[] taggedToDamage;
    [SerializeField] int damage;

    [Header("Travel:")]
    [SerializeField] float travelDistance;
    [SerializeField] float travelTime;
    [SerializeField] float startingHeight;

    #endregion
    #region Hide In Editor

    bool canDamage = true;
    float speed;
    private Coroutine attackCoRoutine;

    #endregion

    #region Unity Functions

    private void Start()
    {
        transform.parent.GetComponent<Trap>().OnTrigger += Attack;

        speed = travelDistance / travelTime;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (canDamage)
        {
            foreach(string tag in taggedToDamage)
            {
                if(tag == col.tag)
                {
                    col.GetComponent<Health>().TakeDamage(damage);
                    canDamage = false;
                }
            }
        }
    }
    
    #endregion
    
    public void Attack()
    {
        if (attackCoRoutine == null)
        {
            attackCoRoutine = StartCoroutine(AttackRoutine());
        }
    }
    private IEnumerator AttackRoutine()
    {
        self.velocity = Vector2.up * speed;
        yield return new WaitForSeconds(travelTime);

        self.velocity = Vector2.down * speed;
        yield return new WaitForSeconds(travelTime);

        self.velocity = Vector2.zero;

        canDamage = true;
        transform.localPosition = new Vector2(transform.localPosition.x, startingHeight);
        attackCoRoutine = null;
    }
}
