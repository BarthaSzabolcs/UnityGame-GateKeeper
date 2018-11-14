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
    public BulletData bulletData;
    public bool chargable;
    [SerializeField] ShootingPattern pattern;
    [SerializeField] float timeBtwShots;

    [SerializeField] bool increasingFireRate;
    [SerializeField] float[] dynamicTimeBtwShots;
    [SerializeField] float fireRateDecreaseTime;
    public bool isAuto;

    [Header("Ammo Settings:")]
    [SerializeField] int extraAmmo;
    public int ammoConsumption;
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
    public int magSize;

    [Header("Reload Settings:")]
    [SerializeField] int reloadAmmount;
    [SerializeField] float reloadTime;
    [SerializeField] int reloadRefreshPerSecond;
    #endregion
    #region HideInEditor
    // components
    Weapon weapon;
    Rigidbody2D self;
    ShootingPattern patternInstance;
    IChargable chargedPattern;

    // timers
    float reloadTimer;
    float fireRateTimer;
    
    // flags
    bool triggerReseted = true;

    int charge = 0;

    // fields managed by property
    [SerializeField] int ammoInMag;
    private int fireRateIndex;

    // Properties
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
    public int FireRateIndex
    {
        get
        {
            return fireRateIndex;
        }
        set
        {
            if (value < 0)
            {
                fireRateIndex = 0;
            }
            else if (value > dynamicTimeBtwShots.Length - 1)
            {
                fireRateIndex = dynamicTimeBtwShots.Length - 1;
            }
            else
            {
                fireRateIndex = value;
            }
        }
    }
    private float TimeBetweenShots
    {
        get
        {
            if (increasingFireRate == true)
            {
                return dynamicTimeBtwShots[FireRateIndex];
            }
            else
            {
                return timeBtwShots;
            }

        }
    }
    #endregion

    public void Initialize(Rigidbody2D self, Weapon weapon)
    {
        this.self = self;
        this.weapon = weapon;
        patternInstance = Instantiate(pattern);

        if(patternInstance is ShootingPattern_CalculatedSpread calculatedSpread)
        {
            calculatedSpread.Initialize(timeBtwShots);
        }
        chargedPattern = patternInstance as IChargable;
    }

    public void PullTrigger()
    {
        if ((triggerReseted == true || isAuto == true) && AmmoInMag > 0 && fireRateTimer + TimeBetweenShots < Time.time)
        {
            if(increasingFireRate)
            {
                FireRateIndex -= (int)Mathf.Floor((Time.time - fireRateTimer) / ( dynamicTimeBtwShots[FireRateIndex] + fireRateDecreaseTime));
                FireRateIndex++;
                Debug.Log(FireRateIndex);
            }

            patternInstance.Shoot(bulletData, self.transform, barrelOffSet);
            AmmoInMag -= ammoConsumption;

            if (muzzleFashAnimation.Length > 0)
            {
                weapon.MuzleFlash();
            }

            charge++;
            triggerReseted = false;
            fireRateTimer = Time.time;
        }
    }
    public void ReleaseTrigger()
    {
        triggerReseted = true;
        if (chargable && AmmoInMag > 0 && chargedPattern != null)
        {
            AmmoInMag -= chargedPattern.ChargedShot(charge, AmmoInMag, bulletData, self.transform, barrelOffSet);
        }
        charge = 0;
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
