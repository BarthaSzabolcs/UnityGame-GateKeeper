using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text moneytext;
    // Start is called before the first frame update
    void Start()
    {
        UpdateMoney();
        UserInterface.Instance.SetWeaponComponent();
        this.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMoney()
    {
        moneytext.text = "Money: " + GameManager.Instance.Money;
    }

    
}
