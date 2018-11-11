using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Health")]
public class HealthData : ScriptableObject
{
    [Header("Health:")]
	public int maxHealthPoints;

    [Header("Hit:")]
    public string hitAudio;
    public bool flashOnHealthChange;
    public float flashTime = 1f;

    [Header("Death:")]
    public string deathAudio;
    public ExplosionData deathAnim;
}
