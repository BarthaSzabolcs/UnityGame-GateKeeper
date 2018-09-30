using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Damage Zone")]
public class DamageZoneData : ScriptableObject {

    [Header("Interaction Settings:")]
    public string[] taggedToDamage;
    public int damage;
    
}
