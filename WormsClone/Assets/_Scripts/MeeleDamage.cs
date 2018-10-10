using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleDamage : MonoBehaviour
{
    [SerializeField] string[] taggedToDamage;
    [SerializeField] float damageFrequency;
    float damageTimer;
    int damage = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        foreach (var tag in taggedToDamage)
        {
            if(tag == col.tag && damageTimer + damageFrequency < Time.time)
            {
                col.GetComponent<Health>().TakeDamage(damage, gameObject);
                damageTimer = Time.time;
            }
        }
    }
}
