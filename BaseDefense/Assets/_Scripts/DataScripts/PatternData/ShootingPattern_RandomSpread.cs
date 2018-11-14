using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/RandomSpread")]
public class ShootingPattern_RandomSpread : ShootingPattern, IChargable
{
    #region ShowInEditor

    [Header("Random Settings:")]
    [SerializeField] float[] randomAngles;
    [SerializeField] float recoilDuration;

    [Header("Spread Settings:")]
    [SerializeField] int bulletCount;
    [SerializeField] float spreadAngle;

    [Header("ChargedShot Settings:")]
    [SerializeField] float chargedRandomSpreadLimit;
    [SerializeField] float chargredAngle;
    #endregion
    #region HideInEditor 
    private float weaponFireRate;
    private float randomSpreadIndex = 0;
    private int RandomSpreadIndex
    {
        get
        {
            if (randomSpreadIndex < 0)
            {
                randomSpreadIndex = 0;
            }
            else if (randomSpreadIndex > (randomAngles.Length - 1) * recoilDuration)
            {
                randomSpreadIndex = (randomAngles.Length - 1) * recoilDuration;
            }

            return Mathf.RoundToInt(randomSpreadIndex / recoilDuration);
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

        randomSpreadIndex -= timeBetweenShots - weaponFireRate - recoilDuration;

        float randomAngle;

        if(bulletCount -1 == 0)
        {
            randomAngle = Random.Range(-0.5f * randomAngles[RandomSpreadIndex], 0.5f * randomAngles[RandomSpreadIndex]);
            Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);
            ShootBullet(bulletData, self, barrelOffSet, rotation);
        }
        else
        {
            for (int i = 0; i < bulletCount; i++)
            {
                randomAngle = Random.Range(-0.5f * randomAngles[RandomSpreadIndex], 0.5f * randomAngles[RandomSpreadIndex]);
                Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);
                
                rotation *= Quaternion.Euler(0, 0, spreadAngle / bulletCount * 0.5f + i * spreadAngle / bulletCount - spreadAngle * 0.5f);
                ShootBullet(bulletData, self, barrelOffSet, rotation);
            }
        }

        previousShot = Time.time;
    }
    public int ChargedShot(int charge, int maxCharge, BulletData bulletData, Transform self, Vector2 barrelOffSet)
    {
            charge = charge <= maxCharge ?
            charge :
            maxCharge;

        int splits = charge;
        if (splits == 0)
        {
            Quaternion rotation = Quaternion.identity;
            if (chargedRandomSpreadLimit != 0)
            {
                rotation = Quaternion.Euler(0, 0, Random.Range(-chargedRandomSpreadLimit, chargedRandomSpreadLimit));
            }
            ShootBullet(bulletData, self, barrelOffSet, rotation);
        }
        else
        {
            for (int i = 0; i < splits; i++)
            {
                Quaternion rotation = Quaternion.identity;
                if (chargedRandomSpreadLimit != 0)
                {
                    rotation = Quaternion.Euler(0, 0, Random.Range(-chargedRandomSpreadLimit, chargedRandomSpreadLimit));
                }

                rotation *= Quaternion.Euler(0, 0, i * chargredAngle / splits - chargredAngle * 0.5f);
                ShootBullet(bulletData, self, barrelOffSet, rotation);
            }
        }

        return charge;
    }
}
