using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/RangedWeapon")]
public class WeaponData : ScriptableObject
{
    #region ShowInEditor
    [Header("Appearance Settings:")]
    public Sprite sprite;
    [SerializeField] Vector2 barrelOffSet;
    public Vector2 weaponPosition;
    public Vector2 rightHandPosition;
    public Vector2 leftHandPosition;
    [Header("Shooting Settings:")]
    [SerializeField] GameObject bullet;
    [SerializeField] BulletData bulletData;
    [SerializeField] ShootingPattern pattern;
    [SerializeField] float fireRate;
    public bool isAuto;
    [Header("Ammo Settings:")]
    public int extraAmmo;
    [SerializeField] int magSize;
    [SerializeField] float reloadTime;
    [SerializeField] int reloadRefreshRate;
    #endregion
    #region HideInEditor
    Weapon weapon;
    Rigidbody2D self;
    float reloadTimer;
    float fireRateTimer;
    public int ammoInMag;
    #endregion

    public void Initialize(Rigidbody2D self, Weapon weapon)
    {
        this.self = self;
        this.weapon = weapon;
    }
    public void Attack()
    {
        if (ammoInMag > 0 && fireRateTimer + fireRate < Time.time)
        {
            pattern.Shoot(bullet, bulletData, self.transform, barrelOffSet);
            fireRateTimer = Time.time;
            ammoInMag--;
        }
    }
    public IEnumerator ReloadRoutine()
    {
        if (extraAmmo > 1)
        {
            for (int i = 0; i < reloadRefreshRate; i++)
            {
                yield return new WaitForSeconds(reloadTime / reloadRefreshRate);
                //Debug.Log("implement reload progress here (" + i / (float)reloadRefreshRate + ")");
                weapon.RefreshReloadBar(i / (float)reloadRefreshRate);
            }
            ammoInMag = extraAmmo >= magSize ? magSize : extraAmmo;
            extraAmmo -= ammoInMag;
        }

        weapon.reloadingRoutine = null;
    }
}
