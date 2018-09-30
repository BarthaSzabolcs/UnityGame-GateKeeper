using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/RangedWeapon")]
public class RangedWeaponData : WeaponData
{
    #region Events
    public delegate void MagChange(int inMag);
    public delegate void ExtraMagChange(int extraMag);
    public event MagChange OnMagChange;
    public event ExtraMagChange OnExtraMagChange;
    public delegate void ReloadChange(float time, float percent);
    public event ReloadChange OnReloadChange;
    #endregion
    #region ShowInEditor
    [Header("RangedWeapon Settings:")]
    [SerializeField] GameObject bullet;
    [SerializeField] BulletData bulletData;
    [SerializeField] ShootingPattern pattern;
    [SerializeField] int ExtraAmmo;
    public int extraAmmo { get { return ExtraAmmo; } set { OnExtraMagChange(value); ExtraAmmo = value;  } }
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
    [SerializeField] int AmmoInMag;
    public int ammoInMag { get { return AmmoInMag; } set { OnMagChange(value); AmmoInMag = value; } }
    private UserInterface uiManager;
    #endregion

    public void Initialize(Rigidbody2D self)
    {
        this.self = self;
        uiManager = UserInterface.Instance;
        OnMagChange = uiManager.MagChange;
        OnExtraMagChange = uiManager.ExtraMagChange;
        OnReloadChange = uiManager.ReloadChange;
        ammoInMag = magSize;
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
            float percent = 1f / reloadRefreshRate;
            var reloadRefreshTime = reloadTime * percent;
            for (int i = 0; i < reloadRefreshRate; i++)
            {
                OnReloadChange(reloadTime - i * reloadRefreshTime, i * percent);
                yield return new WaitForSeconds(reloadRefreshTime);
                //Debug.Log("implement reload progress here (" + i / (float)reloadRefreshRate + ")");
            }
            ammoInMag = extraAmmo >= magSize ? magSize : extraAmmo;
            extraAmmo -= ammoInMag;
        }
        
        weapon.reloadingRoutine = null;
    }

}
