using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] DamageZoneData data;
    #endregion

    private void OnTriggerEnter2D(Collider2D col)
    {
        foreach (var tag in data.taggedToDamage)
        {
            if (tag == col.gameObject.tag)
            {
                col.gameObject.GetComponent<Health>().TakeDamage(data.damage);
            }
        }
    }
}
