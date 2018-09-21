using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/RangedWeapon")]
public class RangedWeaponData : WeaponData
{
    #region ShowInEditor
    [Header("RangedWeapon Settings:")]
    [SerializeField] GameObject bullet;
    [SerializeField] BulletData bulletData;
    [SerializeField] ShootingPattern pattern;
    [SerializeField] int magazineSize;
    [SerializeField] float reloadTime;
    [SerializeField] float fireRate;
    [SerializeField] Vector2 barrelOffSet;
    [SerializeField] int reloadRefreshRate;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    int ammo;
    float reloadTimer;
    float fireRateTimer;
    #endregion

    public void Initialize(Rigidbody2D self)
    {
        this.self = self;
        ammo = magazineSize;
    }
    public override void Attack()
    {
        if(ammo > 0 && fireRateTimer + fireRate < Time.time )
        {
            pattern.Shoot(bullet, bulletData, self.transform, barrelOffSet);
            fireRateTimer = Time.time;
            ammo--;
        }
    }
    public IEnumerator ReloadRoutine(Weapon weapon)
    {
        for (int i = 0; i < reloadRefreshRate; i++)
        {
            yield return new WaitForSeconds(reloadTime / reloadRefreshRate);
            Debug.Log("implement reload progress here (" + i / (float)reloadRefreshRate + ")");
        }
        ammo = magazineSize;
        weapon.reloadingRoutine = null;
    }

}
