using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWeapon : MonoBehaviour
{
    public GameObject weapons;
    public int wepIdx; 
    public WeaponData weapon;
    public int weaponCost;
    public int ammoCost;
    public Image wsprite;
    public Text ammocount;
    public Button buyweapon;
    public Button buyammo;
    public Text buyammotext;
    public Text weaponcostText;
    public Text ammoCostText;
    private Weapon weaponsscript;
    private bool IsBought;
    public bool isBought { get { return IsBought; } set { if (value == true) { buyweapon.interactable = false; buyammo.interactable = true; } else { buyweapon.interactable = true; buyammo.interactable = false; } IsBought = value; } }
    public Text moneyText;


    // Start is called before the first frame update
    void Start()
    {
        StartData();
        UpdateBought();
        UpdateData();
    }

    void StartData()
    {

        weaponsscript = weapons.GetComponent<Weapon>();
        isBought = false;
        //InitTest();

    }
    void UpdateBought()
    {
        wsprite.sprite = weapon.sprite;
        weaponcostText.text = weaponCost.ToString();
        ammoCostText.text = ammoCost.ToString();
        buyammotext.text = "Buy Ammo(" + weapon.AmmoInMag.ToString() + ")";
        UpdateData();
    }

    public void UpdateData()
    {
        moneyText.text = "Money: " + GameManager.Instance.money;
        ammocount.text = (weapon.AmmoInMag + weapon.ExtraAmmo).ToString();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyAmmo()
    {
        //GameManager.Instance.BuyAmmo(weapon, ammoCost);
    }

    //If the player already has the weapon wepIdx != -1

    //public void InitTest()
    //{
    //    if(wepIdx != -1)
    //    {
    //        //weapon = weaponsscript.instances[wepIdx];
    //        isBought = true;
    //    }
        
    //}

    public void BuyWeapon()
    {
        if (wepIdx == -1)
        {
            //wepIdx = GameManager.Instance.BuyWeapon(weapon, weaponCost);
            if (wepIdx != -1)
            {
                ammocount.text = wepIdx.ToString();
                //InitTest();
                UpdateBought();
                UpdateData();
            }
        }
    }

    /*private bool checkIfBought()
    {
        return true;
    }

    void SetBought()
    {
        
    }*/
}
