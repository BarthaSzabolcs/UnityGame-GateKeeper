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
    }

    public void RefreshWeaponSprite(Sprite sprite,WeaponData wdata)
    {
        image.sprite = sprite;
        if(wdata is RangedWeaponData)
        {
            ammo.text = ((RangedWeaponData)wdata).ammoInMag.ToString();
            extraammo.text = ((RangedWeaponData)wdata).extraAmmo.ToString();
            extraammo.enabled = true;
        }
        else
        {
            extraammo.enabled = false;
            ammo.text = "∞";
        }
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
    }

    public void ReloadStop()
    {
        reloadtime.enabled = false;
    }

    public void ReloadChange(float time, float percent)
    {
        reloadtime.text = time.ToString("0.0");
    }

    public void HealthChange(int healthvalue)
    {
        health.text = healthvalue.ToString();
        CalculateHPPercent();
    }

    public void MaxHealthChange(int healthvalue)
    {
        maxhealth.text = healthvalue.ToString();
    }

    private void CalculateHPPercent()
    {
        hppercent = float.Parse(health.text) / float.Parse(maxhealth.text);
    }

    private void RefreshHPBar()
    {

    }





}
