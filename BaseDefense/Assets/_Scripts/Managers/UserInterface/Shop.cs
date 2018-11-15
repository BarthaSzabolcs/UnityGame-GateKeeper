using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{


    #region Show In Editor

    [Header("Items for Sale:")]
    [SerializeField] WeaponData[] weapons;
    [SerializeField] TrapData[] floorTraps;
    [SerializeField] TrapData[] ceilTraps;

    [Header("UI elements:")]
    [SerializeField] GameObject trapMenuItem;
    [SerializeField] RectTransform trapSelect;


    //Trap
    public Trap CurrentTrap { get; private set; }
    public TrapData SelectedTrapInShop { get; private set; }
    Canvas trapMenu_Canvas;
    Text trapName_Text;
    Image trap_Image;
    Text trapLevel_Text;
    #endregion
    #region Hide In Editor

    #endregion
    #region UnityFunctions

    private void Start()
    {
        //trapMenu_Canvas = GameObject.Find(trapMenu_name).GetComponent<Canvas>();
        //trapName_Text = GameObject.Find(trapName_name).GetComponent<Text>();
        //trap_Image = GameObject.Find(trapImage_name).GetComponent<Image>();

        //trapLevel_Text = GameObject.Find(trapLevel_name).GetComponent<Text>();
        //trapLevelUp_Button = GameObject.Find(trapLevelUp_name).GetComponent<Button>();
        //trapLevelDown_Button = GameObject.Find(trapLevelDown_name).GetComponent<Button>();

        trapMenu_Canvas.enabled = false;
    }

    public void OpenTrapShop()
    {

        var item = Instantiate(trapMenuItem, Vector2.zero, Quaternion.identity);

        item.transform.SetParent(trapSelect, false);

    }
    #endregion
}
