using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Pattern_CalculatedSpread")]
public class ShootingPattern_CalculatedSpread : ShootingPattern
{
    #region ShowInEditor
    [Header("Spread Settings:")]
    [SerializeField] float[] angles;
    [SerializeField] float recoilDuration;
    #endregion
    #region HideInEditor 
    private float weaponFireRate;
    private float spreadFloatIndex = 0;
    private int SpreadIndex
    {
        get
        {
            if(spreadFloatIndex < 0)
            {
                spreadFloatIndex = 0;
            }
            else if (spreadFloatIndex > (angles.Length -1) * recoilDuration)
            {
                spreadFloatIndex = (angles.Length - 1) * recoilDuration;
            }

            return Mathf.RoundToInt(spreadFloatIndex / recoilDuration);
        }
    }
    private float previousShot;
    #endregion

    public void Initialize(float timeBetweenShots)
    {
        weaponFireRate = timeBetweenShots;
    }
    public override void Shoot(BulletData bulletData, Transform self, Vector2 barrelOffSet)
	{
        float timeBetweenShots = Time.time - previousShot;

        spreadFloatIndex -= timeBetweenShots - weaponFireRate - recoilDuration;
        Quaternion rotation = Quaternion.Euler(0, 0, angles[SpreadIndex]);
        ShootBullet(bulletData, self, barrelOffSet, rotation);

        previousShot = Time.time;
    }
}
