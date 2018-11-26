using UnityEngine;

[CreateAssetMenu(menuName = "Spawn/Wave")]
public class SpawnWaveData : ScriptableObject
{
    [Header("Enemies:")]
    public EnemyData[] enemies;
}
