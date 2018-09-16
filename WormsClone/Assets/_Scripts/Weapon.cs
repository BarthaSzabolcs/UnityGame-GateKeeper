using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] WeaponData data;
    [SerializeField] PlayerController wielder;
    #endregion
    #region HideInEditor
    WeaponData instance;
    #endregion

    #region UnityFunctions
    private void Start()
    {
        Initialize();
    }
    #endregion
    void Initialize()
    {
        RangedWeaponData rangedData = data as RangedWeaponData;
        if (rangedData != null)
        {
            rangedData = Instantiate(rangedData);
            rangedData.Initialize
            (
                GetComponent<Rigidbody2D>()
            );
            instance = rangedData;
        }

        MeeleWeaponData meeleData = data as MeeleWeaponData;
        if (meeleData != null)
        {
            meeleData = Instantiate(meeleData);
            meeleData.Initialize
            (
            );
            instance = meeleData;
        }

        wielder.OnAttackTriggered += Attack;
        wielder.OnReloadTriggered += Reload;
    }
    void Attack()
    {
        instance.Attack();
    }
    void Reload()
    {
        RangedWeaponData ranged = instance as RangedWeaponData;

        if ( ranged != null)
        {
            ranged.Reload();
        }
        else
        {
            print("weapon can not be reloaded.");
        }
    }

}
