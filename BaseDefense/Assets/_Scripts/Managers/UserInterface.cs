using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    #region ShowInEditor
    [Header("Cursor:")]
    [SerializeField] Texture2D crosshairImage;

    [Header("Weapon:")]
    [SerializeField] string playerWeapon_name;
    [SerializeField] string weaponImage_name;
    [SerializeField] string currentAmmo_name;
    [SerializeField] string extraAmmo_name;

    [Header("Reload:")]
    [SerializeField] string reloadtime_name;
    [SerializeField] string reloadWheelCurrent_name;
    [SerializeField] string reloadwheelBG_name;

    [Header("Health:")]
    [SerializeField] string healthbar_name;
    [SerializeField] string health_name;
    [SerializeField] string maxhealth_name;
         
    [Header("JetPack:")]
    [SerializeField]string fuelbar_name;
    [SerializeField]string fuel_name;
    [SerializeField]string maxfuel_name;

    [Header("BuildMode:")]
    public LayerMask clickLayer;

    [Header("Trap")]
    [SerializeField] string trapMenu_name;
    [SerializeField] string trapName_name;
    [SerializeField] string trapImage_name;
    [SerializeField] string trapLevel_name;
    [SerializeField] string trapLevelUp_name;
    [SerializeField] string trapLevelDown_name;

    [Header("Debug:")]
    [SerializeField] public string debug_name;
    #endregion
    #region HideInEditor
    //Camera
    Camera mainCamera;

    //Ammo
    Image weaponImage_Image;
    Text ammo_Text;
    Text extraAmmo_Text;

    //ReloadWheel
    Text reloadtime_Text;
    Image reloadWheelCurrent_Image;
    Image reloadWheelBackGround_Image;

    //Health
    Image healthBar_Image;
    Text health_Text;
    Text maxHealth_Text;

    //Fuel
    Image fuelBar_Image;
    Text fuel_Text;
    Text maxFuel_Text;

    //Trap
    public Trap CurrentTrap { get; private set; }
    public TrapData SelectedTrapInShop { get; private set; }
    Canvas trapMenu_Canvas;
    Text trapName_Text;
    Image trap_Image;
    Text trapLevel_Text;
    Button trapLevelUp_Button;
    Button trapLevelDown_Button;

    //Debug
    Text debugText;
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
    private int CurrentFuel
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
    private int MaxFuel
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
    private float FuelPercent
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

    //WeaponComp

    public Weapon weaponComponentS;
    #endregion

    #region UnityFunctions
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        TrapClick();
    }
    #endregion
    #region CustomFunctions
    public void InitializeLevelUI()
    {
        mainCamera = Camera.main;

        weaponImage_Image = GameObject.Find(weaponImage_name).GetComponent<Image>();
        ammo_Text = GameObject.Find(currentAmmo_name).GetComponent<Text>();
        extraAmmo_Text = GameObject.Find(extraAmmo_name).GetComponent<Text>();

        reloadtime_Text = GameObject.Find(reloadtime_name).GetComponent<Text>();
        reloadWheelCurrent_Image = GameObject.Find(reloadWheelCurrent_name).GetComponent<Image>();
        reloadWheelBackGround_Image = GameObject.Find(reloadwheelBG_name).GetComponent<Image>();

        healthBar_Image = GameObject.Find(healthbar_name).GetComponent<Image>();
        health_Text = GameObject.Find(health_name).GetComponent<Text>();
        maxHealth_Text = GameObject.Find(maxhealth_name).GetComponent<Text>();

        fuelBar_Image = GameObject.Find(fuelbar_name).GetComponent<Image>();
        fuel_Text = GameObject.Find(fuel_name).GetComponent<Text>();
        maxFuel_Text = GameObject.Find(maxfuel_name).GetComponent<Text>();
        
        debugText = GameObject.Find(debug_name).GetComponent<Text>();

        trapMenu_Canvas = GameObject.Find(trapMenu_name).GetComponent<Canvas>();
        trapName_Text = GameObject.Find(trapName_name).GetComponent<Text>();
        trap_Image = GameObject.Find(trapImage_name).GetComponent<Image>();
        //trapLevel_Text = GameObject.Find(trapLevel_name).GetComponent<Text>();
        //trapLevelUp_Button = GameObject.Find(trapLevelUp_name).GetComponent<Button>();
        //trapLevelDown_Button = GameObject.Find(trapLevelDown_name).GetComponent<Button>();

        trapMenu_Canvas.enabled = false;
        GameManager.Instance.OnBuildModeStateChange += HandleBuildModeChange;

        reloadtime_Text.enabled = false;
        reloadWheelCurrent_Image.enabled = false;
        reloadWheelBackGround_Image.enabled = false;

        Weapon weaponComponent = GameManager.Instance.Player.transform.Find(playerWeapon_name).GetComponent<Weapon>();
        weaponComponent.OnWeaponChanged += RefreshWeaponData;
        weaponComponent.OnReloadStart += ReloadStart;
        weaponComponent.OnReloadChange += Instance.ReloadChange;
        weaponComponent.OnReloadStop += ReloadStop;
        weaponComponent.OnExtraAmmoChange += ExtraMagChange;
        weaponComponent.OnMagChange += MagChange;
        MagChange(weaponComponent.AmmoInMag);
        ExtraMagChange(weaponComponent.ExtraAmmo);

        Health healthComponent = GameManager.Instance.Player.GetComponent<Health>();
        healthComponent.OnHealthCHange += HealthChange;
        healthComponent.OnMaxHealthCHange += MaxHealthChange;
        CurrentHealth = healthComponent.HealthPoint;
        MaxHealth = healthComponent.MaxHealthPoint;

        JetPack jetPackComponent = GameManager.Instance.Player.GetComponent<JetPack>();
        jetPackComponent.OnFuelChange += FuelChange;
        jetPackComponent.OnMaxFuelCHange += MaxFuelChange;
        CurrentFuel = jetPackComponent.Fuel;
        MaxFuel = jetPackComponent.MaxFuel;

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
    //BuildMode
    public void InitializeTrapMenu()
    {
        trapMenu_Canvas.enabled = true;
        trapName_Text.text = CurrentTrap.Data.shopName;
        trap_Image.sprite = CurrentTrap.Data.shopImage;
    }
    private void HandleBuildModeChange(bool state)
    {
        if (state == false)
        {
            CloseTrapMenu();
        }
    }
    private void TrapClick()
    {
        if (GameManager.Instance.InBuildMode && Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ViewportPointToRay(mainCamera.ScreenToViewportPoint(Input.mousePosition));

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 30f,clickLayer);

            if (hit.collider != null)
            {
                OpenTrapMenu(hit.collider.gameObject.GetComponent<Trap>());
            }
        }
    }
    private void OpenTrapMenu(Trap trap)
    {
        CurrentTrap = trap;
        InitializeTrapMenu();
    }
    private void CloseTrapMenu()
    {
        trapMenu_Canvas.enabled = false;
    }
    #endregion

    public void SetWeaponComponent()
    {
        weaponComponentS = GameManager.Instance.Player.transform.Find(playerWeapon_name).GetComponent<Weapon>();
    }

    public bool IncreaseExtraAmmo(WeaponData w)
    {
        if (weaponComponentS.HasWeapon(w))
        {
            WeaponData wep = weaponComponentS.ReturnWeapon(w);
            wep.ExtraAmmo += wep.magSize;
            return true;
        }
        return false;
    }

    public int BuyWeapon(WeaponData w)
    {
        weaponComponentS.AddWeapon(w);
        return weaponComponentS.instances.Count-1;
            
    }
}
