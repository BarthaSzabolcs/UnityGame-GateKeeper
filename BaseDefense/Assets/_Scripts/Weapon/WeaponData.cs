using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    #region ShowInEditor
    [Header("Audio:")]
    [SerializeField] string fireAudio;
    public string reloadAudio;

    [Header("   Idle:")]
    [Header("VisualFX Settings:")]
    public Sprite sprite;

    [Header("   Offsets:")]
    public Vector2 waeponOffSet;
    public Vector2 muzzleFlashOffSet;
    public Vector2 barrelOffSet;
    public Vector2 laserOffSet;
    public Vector2 rightHandPosition;
    public Vector2 leftHandPosition;

    [Header("   Animations:")]
    public AnimationCollection muzzleFashAnimation;
    public AnimationCollection firingAnimation;
    public AnimationCollection reloadAnimation;

    [Header("   Lasersight:")]
    public bool hasLaserSight;
    public Gradient laserSightColor;
    public float laserSightMaxRange;

    [Header("   Hands:")]
    public Sprite rightHandSprite;
    public Sprite leftHandSprite;

    [Header("   Shooting:")]
    [Header("Function Settings:")]
    public BulletData bulletData;
    public bool chargable;
    [SerializeField] ShootingPattern pattern;
    [SerializeField] float timeBtwShots;

    [SerializeField] bool increasingFireRate;
    [SerializeField] float[] dynamicTimeBtwShots;
    [SerializeField] float fireRateDecreaseTime;
    public bool isAuto;

    [Header("   Ammo:")]
    [SerializeField] int extraAmmo;
    public int ammoConsumption;
    public bool infiniteAmmo;
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

    [Header("   Reload:")]
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
    public float LaserSightRange
    {
        get
        {
            float range = bulletData.speed * bulletData.lifeTime;

            return range < laserSightMaxRange ? range : laserSightMaxRange;
        }
    }
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
        if (patternInstance is ShootingPattern_RandomSpread randomSpread)
        {
            randomSpread.Initialize(timeBtwShots);
        }
        chargedPattern = patternInstance as IChargable;
    }

    public void PullTrigger()
    {
        if ((triggerReseted == true || isAuto == true) && AmmoInMag > 0 && fireRateTimer + TimeBetweenShots < Time.time)
        {
            // Calculate new FireRate
            if(increasingFireRate)
            {
                FireRateIndex -= (int)Mathf.Floor((Time.time - fireRateTimer) / ( dynamicTimeBtwShots[FireRateIndex] + fireRateDecreaseTime));
                FireRateIndex++;
            }

            // Shoot Bullet
            patternInstance.Shoot(bulletData, self.transform, barrelOffSet);

            // Consume Ammo
            AmmoInMag -= ammoConsumption;
            
            // Play MuzzleFlash Animation
            weapon.PlayarFiringAnimations(muzzleFashAnimation.Next());
            AudioManager.Instance.PlaySound(fireAudio);

            charge++;
            triggerReseted = false;
            fireRateTimer = Time.time;
        }
    }
    public void ReleaseTrigger()
    {
        triggerReseted = true;

        // CHarged Shot
        if (chargable && AmmoInMag > 0 && chargedPattern != null)
        {
            AmmoInMag -= chargedPattern.ChargedShot(charge, AmmoInMag, bulletData, self.transform, barrelOffSet);
        }

        charge = 0;
    }
    public IEnumerator ReloadRoutine()
    {
        if ((ExtraAmmo >= 1 || infiniteAmmo == true) && ammoInMag != magSize)
        {
            float timePassed = 0;

            // Wait and refresh UI
            while (timePassed < reloadTime)
            {
                weapon.TriggerReloadChange(reloadTime - timePassed, timePassed / reloadTime);

                yield return new WaitForSeconds(1f / reloadRefreshPerSecond);
                timePassed += 1f / reloadRefreshPerSecond;
            }

            // Count bullets to reload
            int difference = magSize - AmmoInMag;

            // Reload Mag
            AmmoInMag += reloadAmmount;

            // Decrease extraAmmo
            if (infiniteAmmo == false)
            {
                ExtraAmmo -= difference < reloadAmmount ? difference : reloadAmmount;
            }
        }
        weapon.ReloadCoroutine = null;
    }
}
