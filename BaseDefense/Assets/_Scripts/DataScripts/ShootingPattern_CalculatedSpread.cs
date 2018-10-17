using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Pattern_CalculatedSpread")]
public class ShootingPattern_CalculatedSpread : ShootingPattern
{
    #region ShowInEditor
    [Header("Spread Settings:")]
    [SerializeField] float[] angles;
    [SerializeField] float spreadResetTime;
    #endregion
    #region HideInEditor 
    int spreadIndex = 10;
    public int SpreadIndex
    {
        get
        {
            return spreadIndex;
        }
        set
        {
            if (value > angles.Length -1)
            {
                spreadIndex = angles.Length -1;
            }
            else if (value < 0)
            {
                spreadIndex = 0;
            }
            else
            {
                spreadIndex = value;
            }
        }
    }
    float spreadResetTimer;
    #endregion

    public override void Shoot(GameObject bullet, BulletData bulletData, Transform self, Vector2 barrelOffSet)
    {
        string msg = SpreadIndex.ToString();

        SpreadIndex -= Mathf.FloorToInt((Time.time - spreadResetTimer) * angles.Length / spreadResetTime);
        Quaternion rotation = Quaternion.Euler(0, 0, angles[SpreadIndex]);

        ShootBullet(bullet, bulletData, self, barrelOffSet, rotation);

        SpreadIndex++;
        spreadResetTimer = Time.time;
    }
}
