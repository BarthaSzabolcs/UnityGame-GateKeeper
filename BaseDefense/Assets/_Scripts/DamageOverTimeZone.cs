using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeZone : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] DamageZoneData data;
    [SerializeField] float timebetweenDamage;
    #endregion
    #region HideIneditor
    float damageTimer;
    #endregion

    private void OnTriggerStay2D(Collider2D col)
    {
        foreach (var tag in data.taggedToDamage)
        {
            if (tag == col.gameObject.tag && damageTimer + timebetweenDamage < Time.time)
            {
                damageTimer = Time.time;
                col.gameObject.GetComponent<Health>().TakeDamage(data.damage, gameObject);
            }
        }
    }
}
