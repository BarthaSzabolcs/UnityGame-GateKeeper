using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    #region Show In Editor

    [Header("Components:")]
    [SerializeField] SpriteRenderer SpriteRenderer;

    [Header("Visual FX:")]
    [SerializeField] float animationSpeed;
    [SerializeField] AnimationCollection begin_animation;
    [SerializeField] AnimationCollection middle_animation;
    [SerializeField] AnimationCollection end_animation;

    [Header("Damage:")]
    [SerializeField] int damageCycles;
    [SerializeField] float damageFrequency;
    [SerializeField] int damage;
    [SerializeField] LayerMask damageMask;

    [Header("Posion")]
    [SerializeField] float poisonDuration;
    [SerializeField] float poisonFrequency;
    [SerializeField] int poisonDamage;

    [Header("Area:")]
    [SerializeField] float radius;

    #endregion
    #region Hide In Editor

    private Coroutine middleRutine;
    Dictionary<Collider2D, float> recentlyDamaged;

    #endregion

    private void Start()
    {
        transform.parent.GetComponent<Trap>().OnTrigger += Attack;
        recentlyDamaged = new Dictionary<Collider2D, float>();
    }
    private void OnDisable()
    {
        transform.parent.GetComponent<Trap>().OnTrigger -= Attack;
    }

    public void Attack()
    {
        StopAllCoroutines();
        StartCoroutine(Begin());
    }

    private IEnumerator Begin()
    {
        foreach(var frame in begin_animation.Next())
        {
            SpriteRenderer.sprite = frame;
            yield return new WaitForSeconds(animationSpeed);
        }

        middleRutine = StartCoroutine(Middle());
        StartCoroutine(DamageArea());
    }
    private IEnumerator Middle()
    {
        while (true)
        {
            for (int i = 0; i < middle_animation.animations[0].frames.Length; i++)
            {
                SpriteRenderer.sprite = middle_animation.animations[0].frames[i];
                yield return new WaitForSeconds(animationSpeed);
            }
            for (int i = middle_animation.animations[0].frames.Length - 1; i >= 0; i--)
            {
                SpriteRenderer.sprite = middle_animation.animations[0].frames[i];
                yield return new WaitForSeconds(animationSpeed);
            }
        }
    }
    private IEnumerator End()
    {
        foreach (var frame in end_animation.Next())
        {
            SpriteRenderer.sprite = frame;
            yield return new WaitForSeconds(animationSpeed);
        }

        SpriteRenderer.sprite = null;
    }

    private IEnumerator DamageArea()
    {
        for(int i = 0; i < damageCycles; i++)
        {
            Damage();
            yield return new WaitForSeconds(damageFrequency);
        }

        StopCoroutine(middleRutine);
        StartCoroutine(End());
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
