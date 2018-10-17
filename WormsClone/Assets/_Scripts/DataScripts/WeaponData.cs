using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WRangedWeapon")]
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
    [SerializeField] int ExtraAmmo;
    public int extraAmmo
    {
        get
        {
            return ExtraAmmo;
        }
        set
        {
            if(OnExtraMagChange != null) OnExtraMagChange(value);
            ExtraAmmo = value;
        }
    }
    [SerializeField] int magSize;
    [SerializeField] float reloadTime;
    [SerializeField] int reloadRefreshRate;
    #endregion
    #region Events
    public delegate void MagChange(int inMag);
    public delegate void ExtraMagChange(int extraMag);
    public event MagChange OnMagChange;
    public event ExtraMagChange OnExtraMagChange;
    public delegate void ReloadChange(float time, float percent);
    public event ReloadChange OnReloadChange;
    #endregion
    #region HideInEditor
    Weapon weapon;
    Rigidbody2D self;
    ShootingPattern patternInstance;
    float reloadTimer;
    float fireRateTimer;
    [SerializeField] int AmmoInMag;
    public int ammoInMag
    {
        get
        {
            return AmmoInMag;
        }
        set
        {
            if(OnMagChange != null) OnMagChange(value);
            AmmoInMag = value;
        }
    }
    private UserInterface uiManager;
    #endregion

    public void Initialize(Rigidbody2D self, Weapon weapon)
    {
        this.self = self;
        //uiManager = UserInterface.Instance;
        //OnMagChange = uiManager.MagChange;
        //OnExtraMagChange = uiManager.ExtraMagChange;
        //OnReloadChange = uiManager.ReloadChange;
        this.weapon = weapon;
        patternInstance = Instantiate(pattern);
    }

    public void Attack()
    {
        if (ammoInMag > 0 && fireRateTimer + fireRate < Time.time)
        {
            patternInstance.Shoot(bullet, bulletData, self.transform, barrelOffSet);
            fireRateTimer = Time.time;
            ammoInMag--;
        }   
    }
    public IEnumerator ReloadRoutine()
    {
        if (extraAmmo > 1)
        {
            float percent = 1f / reloadRefreshRate;
            var reloadRefreshTime = reloadTime * percent;
            for (int i = 0; i < reloadRefreshRate; i++)
            {
                if(OnReloadChange != null)
                {
                    OnReloadChange(reloadTime - i * reloadRefreshTime, i * percent);
                }
                yield return new WaitForSeconds(reloadRefreshTime);
            }
            ammoInMag = extraAmmo >= magSize ? magSize : extraAmmo;
            extraAmmo -= ammoInMag;
        }

        weapon.ReloadRoutine = null;
    }
}
