using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

    public Text health;
    public Text ammo;
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

    private void ChangeMaxHealth()
    {
        maxHealthPoints = player.GetComponent<Health>().maxHealthPoints;
    }

    private void SetSpriteRenderer()
    {
        image = wsprite.GetComponent<Image>();
    }

    private void Initialize()
    {
        ChangeMaxHealth();
        SetSpriteRenderer();
    }

    private void Update()
    {
        RefreshHealth();
        //RefreshPlayerWeapon();
        //RefreshWeapon();
    }

    private void RefreshHealth()
    {
        if (player != null)
        {
            health.text = player.GetComponent<Health>().HealthPoints + "/" + maxHealthPoints;
        }
        else
        {
            health.text = "0/" + maxHealthPoints;
        }
    }

    private void RefreshPlayerWeapon()
    {
        weapons = player.GetComponentInChildren<Weapon>();
    }

    /*private void RefreshWeapon()
    {
        var weapon = weapons.instances[weapons.DataIndex];
        image.sprite = weapon.sprite;
        if (weapon is RangedWeaponData)
        {
            var rangedweapon = weapon as RangedWeaponData;
            ammo.text = rangedweapon.ammoInMag + "/" + rangedweapon.magSize;
        }
        else
        {
            ammo.text = "∞";
        }
    }*/

    public void RefreshReloadTime(float time)
    {
        ammo.text = time.ToString("0.0");
    }

    public void RefreshWeapon(Sprite sprite,bool ranged,int inMag,int rest)
    {
        image.sprite = sprite;
        if(ranged)
        {
            RefreshRangedAmmo(inMag,rest);
        }
        else
        {
            ammo.text = "∞";
        }
    }

    public void RefreshRangedAmmo(int inMag,int rest)
    {
        ammo.text = inMag + "|" + rest;
    }

    
        
    


}
