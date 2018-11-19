using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    #region Show In Editor

    [Header("Damage:")]
    [SerializeField] int damageCycles;
    [SerializeField] float damageFrequency;
    [SerializeField] int damage;
    [SerializeField] LayerMask damageMask;

    [Header("")]
    [SerializeField] float poisonDuration;
    [SerializeField] float poisonFrequency;
    [SerializeField] int poisonDamage;

    [Header("Area:")]
    [SerializeField] float radius;

    #endregion
    #region Hide In Editor

    Dictionary<Collider2D, float> recentlyDamaged;

    #endregion

    private void Start()
    {
        transform.parent.GetComponent<Trap>().OnTrigger += Attack;
        recentlyDamaged = new Dictionary<Collider2D, float>();
    }

    public void Attack()
    {
        StartCoroutine(DamageArea());
    }
    public IEnumerator DamageArea()
    {
        //initialize look

        for(int i = 0; i < damageCycles; i++)
        {
            Damage();
            yield return new WaitForSeconds(damageFrequency);
        }
    }
    private void Damage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, damageMask);

        foreach(Collider2D enemy in enemies)
        {
            var enemyHealth = enemy.GetComponent<Health>();

            enemy.GetComponent<Health>().TakeDamage(damage);

            if (recentlyDamaged.TryGetValue(enemy, out float lastDamaged))
            {
                if(Time.time - lastDamaged >= damageFrequency)
                {
                    enemyHealth.TakeDamgeOverTime
                    (
                        poisonDamage, 
                        (int)Mathf.Round(poisonDuration / poisonFrequency), 
                        poisonFrequency
                    );

                    recentlyDamaged[enemy] = Time.time;
                }
            }
            else
            {
                enemyHealth.TakeDamgeOverTime
                    (
                        poisonDamage,
                        (int)Mathf.Round(poisonDuration / poisonFrequency),
                        poisonFrequency
                    );

                recentlyDamaged.Add(enemy, Time.time);
                enemyHealth.OnDeath += HandleEnemyDeath;
            }
        }
    }

    private void HandleEnemyDeath(GameObject sender)
    {
        recentlyDamaged.Remove(sender.GetComponent<Collider2D>());
    }

}
