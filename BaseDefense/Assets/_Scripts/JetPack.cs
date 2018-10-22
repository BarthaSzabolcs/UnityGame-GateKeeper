using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] Rigidbody2D targetRB;
    [Header("Force Settings:")]
    [SerializeField] float upwardForce;
    [SerializeField] float maxUpwardForce;
    [Header("Fuel Settings")]
    [SerializeField] float maxFuel;
    [SerializeField] float depleteRate;
    [Header("Fuelregen Settings:")]
    [SerializeField] float regenDelay;
    [SerializeField] float regenInterval;
    [SerializeField] float regenRate;
    #endregion
    #region HideInEditor
    float fuelRegenDelayTimer;
    float fuelRegenTimer;
    bool cancelFuelRegen = false;
    float fuel;
    public float Fuel
    {
        get
        {
            return fuel;
        }
        set
        {
            if (value < fuel)
            {
                cancelFuelRegen = true;
            }

            if (value > maxFuel)
            {
                fuel = maxFuel;
            }
            else if (value < 0)
            {
                fuel = 0;
            }
            else
            {
                fuel = value;
            }
        }
    }
    #endregion

    #region UnityFunctions
    private void Start()
    {
        Fuel = maxFuel;
    }
    private void Update()
    {
        FuelRegen();
    }
    #endregion
    public void Use()
    {
        if(fuel != 0)
        {
            if (targetRB.velocity.y < maxUpwardForce)
            {
                targetRB.AddForce(Vector2.up * upwardForce);
                fuel -= depleteRate;
            }
            else
            {
                targetRB.velocity = new Vector2(targetRB.velocity.x, maxUpwardForce);
            }
        }
    }
    //Rework needed
    private void FuelRegen()
    {
        if (cancelFuelRegen)
        {
            cancelFuelRegen = false;
            fuelRegenDelayTimer = Time.time;
        }
        else if (regenDelay + fuelRegenDelayTimer < Time.time)
        {
            if (fuelRegenTimer + regenInterval < Time.time)
            {
                Fuel += regenRate;
                fuelRegenTimer = Time.time;
            }
        }
    }
}
