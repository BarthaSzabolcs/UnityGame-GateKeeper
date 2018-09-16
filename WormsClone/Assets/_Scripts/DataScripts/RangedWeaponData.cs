using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/RangedWeapon")]
public class RangedWeaponData : WeaponData
{
    #region ShowInEditor
    public GameObject bullet;
    public ShootingPattern pattern;
    public int magazineSize;
    public float reloadTime;
    #endregion
    #region HideInEditor
    int ammo;
    Rigidbody2D self;
    #endregion

    public override void Attack()
    {
        pattern.Shoot(bullet, self.transform);
    }
    public void Reload()
    {
        ammo = magazineSize;
    }
    public void Initialize(Rigidbody2D self)
    {
        this.self = self;
    }

}
