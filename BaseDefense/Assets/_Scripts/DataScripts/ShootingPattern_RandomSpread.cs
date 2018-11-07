using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Pattern_Spread")]
public class ShootingPattern_RandomSpread : ShootingPattern
{
    [Header("Spread Settings:")]
    [SerializeField] float randomSpreadLimit;
    [SerializeField] int bulletCount;
    [SerializeField] float angle;

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
}
