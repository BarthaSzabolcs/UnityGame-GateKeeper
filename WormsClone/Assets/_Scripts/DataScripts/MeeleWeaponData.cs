using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/MeeleWeapon")]
public class MeeleWeaponData : WeaponData
{
    #region ShowInEditor
    #endregion
    #region HideInEditor
    #endregion

    public override void Attack()
    {
        Debug.Log("Meele wapon attack not implemented");
    }

    public void Initialize()
    {
        Debug.Log("Meele weapon");
    }

}
