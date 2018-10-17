using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public DamageZoneData data;

    void OnCollisionEnter2D(Collision2D coll)
    {       
        foreach (var tag in data.taggedToDamage)
        {
            if (tag == coll.gameObject.tag)
            {
                coll.gameObject.GetComponent<Health>().TakeDamage(data.damage, gameObject);
            }
        }   
    }

}
