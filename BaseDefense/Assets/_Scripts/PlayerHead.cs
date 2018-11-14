using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    #region Show In Editor
    [SerializeField] SpriteRenderer headRenderer;
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;
    #endregion

    /// <summary>
    /// Rotate the head towards the target
    /// </summary>
    public void LookAt(Vector2 target)
    {
        // Direction to target
        Vector2 direction =  target - (Vector2)transform.position;

        // Head angle to the ground
        float angle = Mathf.Abs(Vector2.Angle(transform.parent.up, direction));

        if (minAngle <= angle && angle <= maxAngle)
        {
            // Rotate the head towards the target based on the difference in angle
            transform.Rotate(new Vector3(0, 0, Vector2.SignedAngle(transform.right, direction)));

            // Check facing direaction
            if (target.x < transform.position.x)
            {
                headRenderer.flipY = true;
            }
            else
            {
                headRenderer.flipY = false;
            }
        }
    }
}
