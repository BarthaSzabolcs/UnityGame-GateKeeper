using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField] Text ammo;
    [SerializeField] Text extraAmmo;
    [SerializeField] Text reloadtime;
    [SerializeField] GameObject player;
    [SerializeField] Image weaponImage;
    [SerializeField] Image HealthBar;
    [SerializeField] Image reloadWheelCurrent;
    [SerializeField] Image reloadWheelBackGround;
    [SerializeField] Text health;
    [SerializeField] Text maxhealth;

    private int currentHealth;
    private int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            CalculateHealthPercent();
            RefreshHealthBar();
        }
    }
    int maxHealth;
    int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
            CalculateHealthPercent();
            RefreshHealthBar();
        }
    }
    float healthPercent;
    float HealthPercent
    {
        get
        {
            return healthPercent;
        }
        set
        {
            healthPercent = value;
            RefreshHealthBar();
        }
    }
    public static UserInterface Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();    
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Initialize()
    {
        reloadtime.enabled = false;
        reloadWheelCurrent.enabled = false;
        reloadWheelBackGround.enabled = false;
    }

    public void RefreshWeaponData(WeaponData wData)
    {
        weaponImage.sprite = wData.sprite;
        ammo.text = wData.ammoInMag.ToString();
        extraAmmo.text = "/" + wData.extraAmmo.ToString();
        extraAmmo.enabled = true;
    }

    public void MagChange(int ammo)
    {
        this.ammo.text = ammo.ToString();
    }
    public void ExtraMagChange(int extraAmmo)
    {
        this.extraAmmo.text = "/" + extraAmmo.ToString();
    }

    public void ReloadStart()
    {
        reloadtime.enabled = true;
        reloadWheelCurrent.enabled = true;
        reloadWheelBackGround.enabled = true;
    }
    public void ReloadStop()
    {
        reloadtime.enabled = false;
        reloadWheelCurrent.enabled = false;
        reloadWheelBackGround.enabled = false;
    }
    public void ReloadChange(float time, float percent)
    {
        reloadtime.text = time.ToString("0.0");
        reloadWheelCurrent.fillAmount = percent;
    }

    public void HealthChange(int healthvalue)
    {
        CurrentHealth = healthvalue;
        health.text = healthvalue.ToString();
    }
    public void MaxHealthChange(int healthvalue)
    {
        MaxHealth = healthvalue;
        maxhealth.text = "/" + healthvalue.ToString();
    }
    private void CalculateHealthPercent()
    {
        HealthPercent = (float)CurrentHealth / MaxHealth;
    }
    private void RefreshHealthBar()
    {
        HealthBar.fillAmount = HealthPercent;
    }
}
