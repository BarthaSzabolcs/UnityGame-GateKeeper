using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    #region Show In Editor

    [Header("Cursors:")]
    [SerializeField] Texture2D cursor_Sell;
    [SerializeField] Texture2D cursor_Build;

    [Header("Items for Sale:")]
    [SerializeField] WeaponData[] weapons;
    [SerializeField] TrapData[] floorTraps;
    [SerializeField] TrapData[] ceilTraps;

    //Trap
    [SerializeField] GameObject trapMenuItem;
    [SerializeField] string trapMenuItem_picturePath;
    [SerializeField] RectTransform trapList;

    [SerializeField] RectTransform trapMenuRect;
    [SerializeField] LayerMask clickLayer;

    [SerializeField] Image currentTrap_Image;
    [SerializeField] Image selectedTrap_Image;
    [SerializeField] Text arrow_Text;

    #endregion
    #region Hide In Editor
    
    Camera mainCamera;
    TrapData[] selectableTraps;

    bool altMode;
    public bool AltMode
    {
        get
        {
            return altMode;
        }
        set
        {
            if (value)
            {
                UserInterface.Instance.ChangeCursor(cursor_Build);
            }
            else
            {
                UserInterface.Instance.ChangeCursor(cursor_Sell);
            }
            altMode = value;
        }
    }

    private int trapIndex;
    private int TrapIndex
    {
        get
        {
            return trapIndex;
        }
        set
        {
            if( value > selectableTraps.Length - 1)
            {
                trapIndex = selectableTraps.Length - 1;
            }
            else if(value < 0)
            {
                trapIndex = 0;
            }
            else
            {
                trapIndex = value;
            }
        }    
    }
    private TrapData SelectedTrap
    {
        get
        {
            return selectableTraps[TrapIndex];
        }
    }

    Trap currentTrap;
    public Trap CurrentTrap
    {
        get
        {
            return currentTrap;
        }
        set
        {
            trapMenuRect.gameObject.SetActive(value == null);
           
            if(value != currentTrap)
            {
                if (value?.type == Trap.Type.Floor)
                {
                    selectableTraps = floorTraps;
                }
                else if (value?.type == Trap.Type.Ceil)
                {
                    selectableTraps = ceilTraps;
                }
            }
            currentTrap = value;
        }
    }

    #endregion
    #region UnityFunctions

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        CheckAltMode();
        CheckHover();
        CheckTrapChange();
        TrapClick();
        PositionateTrapMenu();
        //ChangeSelectedTrap();
    }

    #endregion

    public void SetupItemList()
    {
        for (int i = 0; i < trapList.childCount; i++)
        {
            Destroy(trapList.GetChild(i).gameObject);
        }

        foreach (TrapData trap in selectableTraps)
        {
            AddItemToList(trap);
        }
        
        TrapIndex = trapIndex;
    }
    private void AddItemToList(TrapData trapData)
    {
        var trap = Instantiate(trapMenuItem, Vector2.zero, Quaternion.identity);

        trap.transform.SetParent(trapList, false);
        trap.transform.Find(trapMenuItem_picturePath).GetComponent<Image>().sprite = trapData.shopImage;
    }

    private void CheckAltMode()
    {
        if (Input.GetButtonDown("BuildMode_Shift"))
        {
            AltMode = true;
        }
        else if (Input.GetButtonUp("BuildMode_Shift"))
        {
            AltMode = false;
        }
    }
    private void PositionateTrapMenu()
    {
        if (CurrentTrap != null)
        {
            trapMenuRect.position = mainCamera.WorldToScreenPoint(CurrentTrap.transform.position) + CurrentTrap.UIoffset;
        }
    }
    private void RefreshTrapMenu()
    {
        if (AltMode)
        {
            currentTrap_Image.sprite = CurrentTrap.Data.sprite;
            selectedTrap_Image.sprite = null;

            arrow_Text.text = CurrentTrap.Data.shopSellingPrice.ToString();
        }
        else
        {
            currentTrap_Image.sprite = CurrentTrap.Data.sprite;
            selectedTrap_Image.sprite = SelectedTrap.shopImage;

            arrow_Text.text = (CurrentTrap.Data.shopSellingPrice - SelectedTrap.shopPrice).ToString();
        }
    }

    private void CheckTrapChange()
    {
        if(Input.GetButtonDown("NextWeapon"))
        {
            TrapIndex--;
        }
        else if (Input.GetButtonDown("PreviousWeapon"))
        {
            TrapIndex++;
        }
    }
    private void CheckHover()
    {

        Ray ray = mainCamera.ViewportPointToRay(mainCamera.ScreenToViewportPoint(Input.mousePosition));

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 30f, clickLayer);

        if (hit.collider != null)
        {
            CurrentTrap = hit.collider.gameObject.GetComponent<Trap>();
        }
        else
        {
            CurrentTrap = null;
        }

    }
    private void TrapClick()
    {

        if (Input.GetMouseButtonDown(0) && CurrentTrap != null)
        {
            if (AltMode)
            {

                GameManager.Instance.money += CurrentTrap.Data.shopSellingPrice;
                CurrentTrap.Data = null;

            }
            else /*if (GameManager.Instance.money - SelectedItem.shopPrice >= 0)*/
            {

                GameManager.Instance.money += CurrentTrap.Data.shopSellingPrice;
                GameManager.Instance.money -= SelectedTrap.shopPrice;

                CurrentTrap.Data = SelectedTrap;
            }
        }

    }

}
