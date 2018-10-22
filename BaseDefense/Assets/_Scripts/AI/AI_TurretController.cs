using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TurretController : MonoBehaviour
{

    #region ShowInEditor
   [SerializeField] AI_Sight sight;
   [SerializeField] Weapon weapon;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    bool shoot;
    #endregion

    void Start()
    {
        self = GetComponent<Rigidbody2D>();
        sight.OnTargetFound += HandleTargetFound;
        sight.OnTargetLost += HandleTargetLost;
    }
    void Update()
    {
        if (shoot)
        {
            weapon.Attack();
        }
    }

    void HandleTargetFound()
    {
        shoot = true;
    }
    void HandleTargetLost()
    {
        shoot = false;
    }
}
