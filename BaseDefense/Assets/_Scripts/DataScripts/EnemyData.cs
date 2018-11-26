using UnityEngine;

[CreateAssetMenu(menuName = "Spawn/Enemy")]
public class EnemyData : ScriptableObject
{
    public enum Type { Ground, Air };
    public enum Rarity { Common, Rare, Epic, Legendary };

    [Header("Enemy:")]
    public GameObject enemy;

    [Header("Spawn:")]
    public Type type;
    public Rarity rarity;
    public float spawndelay;

    [Header("Balance:")]
    public int strengh;
}