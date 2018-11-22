using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text moneytext;

    void Start()
    {
        UpdateMoney();

        //UserInterface.Instance.SetWeaponComponent();    
    }
    
    public void UpdateMoney()
    {
        moneytext.text = "Money: " + GameManager.Instance.Money;
    }
}
