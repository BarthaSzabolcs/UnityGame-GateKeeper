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
    public int extraAmmo;
    public int magSize;
    [SerializeField] float reloadTime;
    [SerializeField] float fireRate;
    [SerializeField] Vector2 barrelOffSet;
    [SerializeField] int reloadRefreshRate;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    float reloadTimer;
    float fireRateTimer;
    public int ammoInMag;
    private UserInterface uiManager;
    #endregion

    public void Initialize(Rigidbody2D self)
    {
        this.self = self;
        ammoInMag = magSize;
        uiManager = UserInterface.Instance;
    }
    public override void Attack()
    {
        if(ammoInMag > 0 && fireRateTimer + fireRate < Time.time )
        {
            pattern.Shoot(bullet, bulletData, self.transform, barrelOffSet);
            fireRateTimer = Time.time;
            ammoInMag--;
        }
    }
    public IEnumerator ReloadRoutine(Weapon weapon)
    {
        if(extraAmmo > 1)
        {
            var reloadRefreshTime = reloadTime / reloadRefreshRate;
            for (int i = 0; i < reloadRefreshRate; i++)
            {
                uiManager.RefreshReloadTime(reloadTime - i * reloadRefreshTime);
                yield return new WaitForSeconds(reloadRefreshTime);
                //Debug.Log("implement reload progress here (" + i / (float)reloadRefreshRate + ")");
            }
            ammoInMag = extraAmmo >= magSize ? magSize : extraAmmo;
            extraAmmo -= ammoInMag;
        }
        
        weapon.reloadingRoutine = null;
    }

}
