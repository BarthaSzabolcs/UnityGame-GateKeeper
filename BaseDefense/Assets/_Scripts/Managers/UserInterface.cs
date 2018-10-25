using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    #region ShowInEditor
    [Header("Cursor:")]
    [SerializeField] Texture2D crosshairImage;

    [Header("Player components:")]
    [SerializeField] Weapon weaponComponent;
    [SerializeField] Health healthComponent;
    [SerializeField] JetPack jetPackComponent;

    [Header("Weapon:")]
    [SerializeField] Image weaponImage_Image;
    [SerializeField] Text ammo_Text;
    [SerializeField] Text extraAmmo_Text;

    [Header("Reload:")]
    [SerializeField] Text reloadtime_Text;
    [SerializeField] Image reloadWheelCurrent_Image;
    [SerializeField] Image reloadWheelBackGround_Image;

    [Header("Health:")]
    [SerializeField] Image healthBar_Image;
    [SerializeField] Text health_Text;
    [SerializeField] Text maxHealth_Text;

    [Header("JetPack:")]
    [SerializeField] Image fuelBar_Image;
    [SerializeField] Text fuel_Text;
    [SerializeField] Text maxFuel_Text;

    [Header("Debug:")]
    [SerializeField] Text debugText;
    #endregion
    #region HideInEditor
    public static UserInterface Instance { get; private set; }
    //Health
    private int currentHealth;
    int maxHealth;
    float healthPercent;
    private int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            HealthPercent = (float)CurrentHealth / MaxHealth;
        }
    }
    int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
            HealthPercent = (float)CurrentHealth / MaxHealth;
        }
    }
    float HealthPercent
    {
        get
        {
            return healthPercent;
        }
        set
        {
            healthPercent = value;
            healthBar_Image.fillAmount = HealthPercent;
        }
    }
    //Fuel
    private int currentFuel;
    private int maxFuel;
    private float fuelPercent;
    public int CurrentFuel
    {
        get
        {
            return currentFuel;
        }
        set
        {
            currentFuel = value;
            FuelPercent = (float)currentFuel / MaxFuel;
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
            FuelPercent = (float)CurrentFuel / maxFuel;
        }
    }
    public float FuelPercent
    {
        get
        {
            return fuelPercent;
        }
        set
        {
            fuelPercent = value;
            fuelBar_Image.fillAmount = fuelPercent;
        }
    }
    #endregion

    private void Awake()
    {
        if (Instance == null)
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
        reloadtime_Text.enabled = false;
        reloadWheelCurrent_Image.enabled = false;
        reloadWheelBackGround_Image.enabled = false;

        weaponComponent.OnWeaponChanged += RefreshWeaponData;
        weaponComponent.OnReloadStart += ReloadStart;
        weaponComponent.OnReloadChange += Instance.ReloadChange;
        weaponComponent.OnReloadStop += ReloadStop;
        weaponComponent.OnExtraAmmoChange += ExtraMagChange;
        weaponComponent.OnMagChange += MagChange;

        healthComponent.OnHealthCHange += HealthChange;
        healthComponent.OnMaxHealthCHange += MaxHealthChange;

        jetPackComponent.OnFuelChange += FuelChange;
        jetPackComponent.OnMaxFuelCHange += MaxFuelChange;

        ChangeCursorToCrossHair();
    }
    //Cursor
    public void ChangeCursorToCrossHair()
    {
        Cursor.SetCursor(crosshairImage, new Vector2(crosshairImage.width/2, crosshairImage.height/2), CursorMode.Auto);
    }
    public void ChangeCursorBack()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    //Weapon
    private void RefreshWeaponData(WeaponData wData)
    {
        weaponImage_Image.sprite = wData.sprite;
        ammo_Text.text = wData.AmmoInMag.ToString();
        extraAmmo_Text.text = "/" + wData.ExtraAmmo.ToString();
        extraAmmo_Text.enabled = true;
    }
    //Ammo
    private void MagChange(int value)
    {
        this.ammo_Text.text = value.ToString();
    }
    private void ExtraMagChange(int value)
    {
        this.extraAmmo_Text.text = "/" + value.ToString();
    }
    //Reload
    private void ReloadStart()
    {
        reloadtime_Text.enabled = true;
        reloadWheelCurrent_Image.enabled = true;
        reloadWheelBackGround_Image.enabled = true;
    }
    private void ReloadStop()
    {
        reloadtime_Text.enabled = false;
        reloadWheelCurrent_Image.enabled = false;
        reloadWheelBackGround_Image.enabled = false;
    }
    private void ReloadChange(float time, float percent)
    {
        reloadtime_Text.text = time.ToString("0.0");
        reloadWheelCurrent_Image.fillAmount = percent;
    }
    //Health
    private void HealthChange(int value)
    {
        CurrentHealth = value;
        health_Text.text = value.ToString();
    }
    private void MaxHealthChange(int value)
    {
        MaxHealth = value;
        maxHealth_Text.text = "/" + value.ToString();
    }
    //Fuel
    private void FuelChange(int value)
    {
        CurrentFuel = value;
        fuel_Text.text = value.ToString();
    }
    private void MaxFuelChange(int value)
    {
        MaxFuel = value;
        maxFuel_Text.text = "/" + value.ToString();
    }
    //Debug
    public void DebugLog(string text)
    {
        debugText.text = text;
    }
}
