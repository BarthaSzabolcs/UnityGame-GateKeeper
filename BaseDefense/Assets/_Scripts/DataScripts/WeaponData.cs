using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponData")]
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
    [SerializeField] int extraAmmo;
    public int ExtraAmmo
    {
        get
        {
            return extraAmmo;
        }
        set
        {
            extraAmmo = value;
            weapon.TriggerExtraAmmoChange(extraAmmo);
        }
    }
    [SerializeField] int magSize;
    [SerializeField] float reloadTime;
    [SerializeField] int reloadRefreshPerSecond;
    #endregion
    #region HideInEditor
    Weapon weapon;
    Rigidbody2D self;
    ShootingPattern patternInstance;
    float reloadTimer;
    float fireRateTimer;
    [SerializeField] int ammoInMag;
    public int AmmoInMag
    {
        get
        {
            return ammoInMag;
        }
        set
        {
            ammoInMag = value;
            weapon.TriggerMagChange(ammoInMag);
        }
    }
    #endregion

    public void Initialize(Rigidbody2D self, Weapon weapon)
    {
        this.self = self;
        this.weapon = weapon;
        patternInstance = Instantiate(pattern);
    }

    public void Attack()
    {
        if (AmmoInMag > 0 && fireRateTimer + fireRate < Time.time)
        {
            patternInstance.Shoot(bullet, bulletData, self.transform, barrelOffSet);
            fireRateTimer = Time.time;
            AmmoInMag--;
        }
    }
    public IEnumerator ReloadRoutine()
    {
        if (ExtraAmmo >= 1)
        {
            float timePassed = 0;
            while (timePassed < reloadTime)
            {
                weapon.TriggerReloadChange(reloadTime - timePassed, timePassed / reloadTime);

                yield return new WaitForSeconds(1f / reloadRefreshPerSecond);
                timePassed += 1f / reloadRefreshPerSecond;
            }
            AmmoInMag = ExtraAmmo >= magSize ? magSize : ExtraAmmo;
            ExtraAmmo -= AmmoInMag;
        }
        weapon.ReloadRoutine = null;
    }
}
