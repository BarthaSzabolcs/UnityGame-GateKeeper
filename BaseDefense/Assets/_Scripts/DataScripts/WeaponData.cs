using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    #region ShowInEditor
    [Header("Appearance Settings:")]
    public Sprite sprite;
    public Sprite[] muzzleFashAnimation;
    [SerializeField] Vector2 barrelOffSet;
    public Vector2 weaponPosition;
    public Vector2 rightHandPosition;
    public Vector2 leftHandPosition;

    [Header("Shooting Settings:")]
    [SerializeField] GameObject bullet;
    public BulletData bulletData;
    [SerializeField] ShootingPattern pattern;
    [SerializeField] float timeBetweenShots;
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

    [Header("Reload Settings:")]
    [SerializeField] int reloadAmmount;
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
            ammoInMag = value > magSize ? magSize : value;
            weapon.TriggerMagChange(ammoInMag);
        }
    }
    #endregion

    public void Initialize(Rigidbody2D self, Weapon weapon)
    {
        this.self = self;
        this.weapon = weapon;
        patternInstance = Instantiate(pattern);

        //maybe not the best aproach
        var calculatedSpread = patternInstance as ShootingPattern_CalculatedSpread;
        if(calculatedSpread)
        {
            calculatedSpread.Initialize(timeBetweenShots);
        }
    }

    public void Attack()
    {
        if (AmmoInMag > 0 && fireRateTimer + timeBetweenShots < Time.time)
        {
            if(muzzleFashAnimation.Length > 0)
            {
                weapon.MuzleFlash();
            }
            patternInstance.Shoot(bullet, bulletData, self.transform, barrelOffSet);
            fireRateTimer = Time.time;
            AmmoInMag--;
        }
    }
    public IEnumerator ReloadRoutine()
    {
        if (ExtraAmmo >= 1 && ammoInMag != magSize)
        {
            float timePassed = 0;
            while (timePassed < reloadTime)
            {
                weapon.TriggerReloadChange(reloadTime - timePassed, timePassed / reloadTime);

                yield return new WaitForSeconds(1f / reloadRefreshPerSecond);
                timePassed += 1f / reloadRefreshPerSecond;
            }

            int difference = magSize - AmmoInMag;
            AmmoInMag += reloadAmmount;
            ExtraAmmo -= difference < reloadAmmount ? difference : reloadAmmount;
        }
        weapon.ReloadRoutine = null;
    }
}
