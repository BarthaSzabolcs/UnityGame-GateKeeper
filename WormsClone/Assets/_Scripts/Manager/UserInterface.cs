using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

    public Text health;
    public Text maxhealth;
    private float Hppercent;
    private float hppercent { get { return Hppercent; } set { Hppercent = value; RefreshHPBar(); } }
    public Text ammo;
    public Text extraammo;
    public Text reloadtime;
    public GameObject player;
    public Weapon weapons;
    public GameObject wsprite;
    private Image image;
    private int maxHealthPoints;
    public Image HealthBar;
    public Image reloadWheel;
    [SerializeField] List<Sprite> reloadsprites;


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
        image = wsprite.GetComponent<Image>();
        reloadtime.enabled = false;
        reloadWheel.enabled = false;
    }

    public void RefreshWeaponSprite(Sprite sprite,WeaponData wdata)
    {
        image.sprite = sprite;
        
            ammo.text = wdata.ammoInMag.ToString();
            extraammo.text = wdata.extraAmmo.ToString();
            extraammo.enabled = true;
        
    }

    public void MagChange(int inMag)
    {
        ammo.text = inMag.ToString();
    }

    public void ExtraMagChange(int extraMag)
    {
        extraammo.text = extraMag.ToString();
    }

    public void ReloadStart()
    {
        reloadtime.enabled = true;
        reloadWheel.enabled = true;
    }

    public void ReloadStop()
    {
        reloadtime.enabled = false;
        reloadWheel.enabled = false;
    }

    public void ReloadChange(float time, float percent)
    {
        reloadtime.text = time.ToString("0.0");
        if(percent < 0.125)
        {
            reloadWheel.sprite = reloadsprites[0];
        }
        else if(percent < 0.375)
        {
            reloadWheel.sprite = reloadsprites[1];
        }
        else if (percent < 0.625)
        {
            reloadWheel.sprite = reloadsprites[2];
        }
        else if (percent < 0.875)
        {
            reloadWheel.sprite = reloadsprites[3];
        }
        else
        {
            reloadWheel.sprite = reloadsprites[4];
        }
    }

    public void HealthChange(int healthvalue)
    {
        health.text = healthvalue.ToString();
        CalculateHPPercent();
    }

    public void MaxHealthChange(int healthvalue)
    {
        maxhealth.text = healthvalue.ToString();
        CalculateHPPercent();
    }

    private void CalculateHPPercent()
    {
        hppercent = float.Parse(health.text) / float.Parse(maxhealth.text);
    }

    private void RefreshHPBar()
    {
        HealthBar.rectTransform.localScale = new Vector3(hppercent, 1, 0);
    }





}
