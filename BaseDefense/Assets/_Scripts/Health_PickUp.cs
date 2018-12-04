using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_PickUp : MonoBehaviour
{

    [Header("Settings:")]
    [SerializeField] string[] taggedToHeal;
    [SerializeField] int healAmmount;

    private void OnCollisionEnter2D(Collision2D col)
    {
        foreach (var tag in taggedToHeal)
        {
            if (col.gameObject.tag == tag)
            {
                col.gameObject.GetComponent<Health>().GainHealth(healAmmount);
                ObjectPool.Instance.Pool(ObjectPool.Types.healthDrop, gameObject);
                break;
            }
        }
    }

}
