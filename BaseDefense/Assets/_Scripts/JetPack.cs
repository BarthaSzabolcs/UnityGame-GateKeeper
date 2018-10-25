using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    #region Events
    public delegate void FuelChange(int value);

    public event FuelChange OnFuelChange;
    public event FuelChange OnMaxFuelCHange;
    #endregion
    #region ShowInEditor
    [SerializeField] Rigidbody2D targetRB;
    [Header("Force Settings:")]
    [SerializeField] float upwardForce;
    [SerializeField] float maxUpwardVelocity;

    [Header("Fuel Settings")]
    [SerializeField] int maxFuel;
    [SerializeField] int depleteRate;

    [Header("Fuelregen Settings:")]
    [SerializeField] float regenDelay;
    [SerializeField] float regenInterval;
    [SerializeField] int regenRate;
    #endregion
    #region HideInEditor
    private float fuelRegenDelayTimer;
    private float fuelRegenTimer;
    private Coroutine fuelRegenRoutine;
    int fuel;
    public int Fuel
    {
        get
        {
            return fuel;
        }
        set
        {
            if (value < fuel)
            {
                if (fuelRegenRoutine != null)
                {
                    StopCoroutine(fuelRegenRoutine);
                }
                fuelRegenRoutine = StartCoroutine(FuelRegenRoutine());
            }

            if (value > MaxFuel)
            {
                fuel = MaxFuel;
            }
            else if (value < 0)
            {
                fuel = 0;
            }
            else
            {
                fuel = value;
            }
            OnFuelChange?.Invoke(fuel);
        }
    }
    public int MaxFuel
    {
        get
        {
            return maxFuel;
        }
        set
        {
            maxFuel = value;
            OnMaxFuelCHange.Invoke(maxFuel);
        }
    }
    #endregion

    #region UnityFunctions
    private void Start()
    {
        MaxFuel = MaxFuel;
        Fuel = MaxFuel;
    }
    #endregion
    public void Use()
    {
        if(Fuel > 0)
        {
            if (targetRB.velocity.y < maxUpwardVelocity)
            {
                targetRB.AddForce(Vector2.up * upwardForce);
                Fuel -= depleteRate;
            }
            else
            {
                targetRB.velocity = new Vector2(targetRB.velocity.x, maxUpwardVelocity);
            }
        }
    }

    private IEnumerator FuelRegenRoutine()
    {
        yield return new WaitForSeconds(regenDelay);
        while(Fuel < MaxFuel)
        {
            Fuel += regenRate;
            yield return new WaitForSeconds(regenInterval);
        }
    }
}
