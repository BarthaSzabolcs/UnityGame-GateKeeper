using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Flying text")]

public class FlyingTextData : ScriptableObject
{
    [Header("Image:")]
    public Sprite displayedImage;

    [Header("Normal:")]
    public Color color;
    public float lifeTime;

    [Header("Fadout:")]
    public Vector2 velocity;
    public float fadeOutDuration;
}
