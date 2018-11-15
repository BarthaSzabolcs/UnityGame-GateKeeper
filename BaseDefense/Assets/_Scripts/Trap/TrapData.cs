using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/Trap")]
public class TrapData : ScriptableObject
{
    [Header("Shop Settings:")]
    public Sprite shopImage; 
    public string shopName;
    public int shopPrice;
    public int shopSellingPrice;
    public string shopDescription;

    [Header("Interaction Settings:")]
    public TrapBehaviour trapBehaviour;
    public string[] taggedToDamage;
    public int damage;
    public Vector2 triggerZoneSize;
    public Vector2 triggerZoneOffset;
    public float recoverSpeed;

    [Header("Effects Settings:")]
    public Sprite sprite;
    public Sprite buildModeSprite;
    public string recoverAudio;
    public string triggerAudio;
}
