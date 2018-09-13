using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/DestructableDataHolder")]
public class DestructableDataHolder : ScriptableObject
{
    [Header("Health:")]
	public int maxHealth;

    [Header("Hit:")]
    public string hitAudio;
    public bool flashOnHealthChange;
    public float flashTime = 1f;

    [Header("Death:")]
    public string deathAudio;
    public GameObject deathAnim;
    public ExplosionDataHolder deathAnimData;
}
