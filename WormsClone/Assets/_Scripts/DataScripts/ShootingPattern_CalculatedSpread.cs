using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Pattern_CalculatedSpread")]
public class ShootingPattern_CalculatedSpread : ShootingPattern
{
    [Header("Spread Settings:")]
    [SerializeField] float[] angles;
    [SerializeField] float spreadResetTime;
    #region HideInEditor
    int rotationIterator = 0;
    float spreadResetTimer;
    #endregion
    public override void Shoot(GameObject bullet, BulletData bulletData, Transform self, Vector2 barrelOffSet)
    {
        Quaternion rotation;
        if (spreadResetTimer + spreadResetTime > Time.time)
        {
            rotation = Quaternion.Euler(0, 0, angles[rotationIterator++]);
        }
        else
        {
            rotationIterator = 0;
            rotation = Quaternion.Euler(0, 0, angles[rotationIterator]);
        }
        ShootBullet(bullet, bulletData, self, barrelOffSet, rotation);
        spreadResetTimer = Time.time;
    }
}
