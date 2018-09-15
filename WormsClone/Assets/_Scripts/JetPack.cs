using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] Rigidbody2D targetRB;
    [Header("Jetpack Settings:")]
    [SerializeField] float upwardForce;
    [SerializeField] float maxFuel;
    [SerializeField] float regenDelay;
    [SerializeField] float regenInterval;
    [SerializeField] float depleteRate;
    [SerializeField] float regenRate;
    #endregion
    #region HideInEditor
    float fuelRegenDelayTimer;
    float fuelRegenTimer;
    bool cancelFuelRegen = false;
    [SerializeField] float fuel;
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

    #region Magic
    private void Start()
    {
        targetRB.GetComponent<PlayerMovement>().OnJetPackTriggered += JetPackHandler;
        Fuel = maxFuel;
    }
    private void Update()
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
    #endregion
    private void JetPackHandler()
    {
        if(fuel != 0)
        {
            targetRB.AddForce(Vector2.up * upwardForce);
            Fuel -= depleteRate;
        }
    }
}
