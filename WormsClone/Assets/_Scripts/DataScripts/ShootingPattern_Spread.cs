using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SpreadPattern")]
public class ShootingPattern_Spread : ShootingPattern
{
    [Header("Spread Settings:")]
    [SerializeField] float spread;

    public override void Shoot(GameObject bullet, BulletData bulletData, Transform self, Vector2 barrelOffSet)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-spread, spread));
        ShootBullet(bullet, bulletData, self, barrelOffSet, rotation);
    }
}
