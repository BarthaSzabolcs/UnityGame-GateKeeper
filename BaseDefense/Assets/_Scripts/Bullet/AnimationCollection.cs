using UnityEngine;

[CreateAssetMenu(fileName = "new AnimationCollection", menuName = "Data/AnimationCollection")]
public class AnimationCollection: ScriptableObject
{
    [Header("Animations:")]
    public Animation[] animations;

    [Header("Rotation")]
    [SerializeField] bool randomizeX;
    [SerializeField] bool randomizeY;

    /// <summary>
    /// Returns a random animation from the array
    /// </summary>
    /// <returns></returns>
    public Sprite[] Next()
    {
        return animations[Random.Range(0, animations.Length)].frames;
    }

    /// <summary>
    /// Randomly flips the renderer X and Y based on the AnimationCollection settings 
    /// </summary>
    /// <param name="spriteRenderer"></param>
    public void Randomize(SpriteRenderer spriteRenderer)
    {
        if (randomizeX)
        {
            spriteRenderer.flipX = Random.Range(0, 1) > 0 ? true : false;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        if (randomizeY)
        {
            spriteRenderer.flipY = Random.Range(0, 1) > 0 ? true : false;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }
}

[System.Serializable]
public class Animation
{
    public Sprite[] frames;
}