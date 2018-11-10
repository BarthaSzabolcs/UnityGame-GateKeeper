using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TurretController : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] AI_Sight sight;
    [SerializeField] Weapon weapon;
    [Header("Aim:")]
    [SerializeField] float maxRotationAngle;
    [SerializeField] float aimingPrecision;
    #endregion
    #region HideInEditor
    Rigidbody2D self;

    bool shoot;
    bool reload;
    #endregion

    void Start()
    {
        self = GetComponent<Rigidbody2D>();
        sight.OnTargetFound += HandleTargetFound;
        sight.OnTargetLost += HandleTargetLost;
        weapon.OnMagEmpty += HandleMagEmpty;
        weapon.OnReloadStop += HandleReloadStopped;
    }
    void Update()
    {
        if (sight.Target && sight.TargetVisible)
        {
            RotateTowardsTarget();
        }

        if (shoot && !reload && Mathf.Abs(sight.LineOfSightAngle) < aimingPrecision)
        {
            weapon.PullTrigger();
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
    void HandleMagEmpty()
    {
        reload = true;
        weapon.Reload();
    }
    void HandleReloadStopped()
    {
        reload = false;
    }

    void RotateTowardsTarget()
    {
        float rotation = 
            Mathf.Abs(sight.LineOfSightAngle) < maxRotationAngle ?
            sight.LineOfSightAngle :
            Mathf.Sign(sight.LineOfSightAngle) * maxRotationAngle;

        Vector3 rotationVector = new Vector3(0, 0, rotation);
        transform.Rotate(rotationVector);
    }
}
