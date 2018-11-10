using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/RandomSpread")]
public class ShootingPattern_RandomSpread : ShootingPattern, IChargable
{
    [Header("Spread Settings:")]
    [SerializeField] float randomSpreadLimit;
    [SerializeField] int bulletCount;
    [SerializeField] float angle;

    [Header("ChargedShot Settings:")]
    [SerializeField] float chargedRandomSpreadLimit;
    [SerializeField] float chargredAngle;

    public override void Shoot(BulletData bulletData, Transform self, Vector2 barrelOffSet)
	{
        int splits = bulletCount - 1;
        if(splits == 0)
        {
            Quaternion rotation = Quaternion.identity;
            if (randomSpreadLimit != 0)
            {
                rotation = Quaternion.Euler(0, 0, Random.Range(-randomSpreadLimit, randomSpreadLimit));
            }
            ShootBullet(bulletData, self, barrelOffSet, rotation);
        }
        else
        {
            for (int i = 0; i < splits; i++)
            {
                Quaternion rotation = Quaternion.identity;
                if (randomSpreadLimit != 0)
                {
                    rotation = Quaternion.Euler(0, 0, Random.Range(-randomSpreadLimit, randomSpreadLimit));
                }

                rotation *= Quaternion.Euler(0, 0, i * angle / splits - angle * 0.5f);
                ShootBullet(bulletData, self, barrelOffSet, rotation);
            }
        }
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
