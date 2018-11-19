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

    [Header("Trap menu item:")]
    [SerializeField] GameObject trapMenuItem;
    [SerializeField] string trapMenuItem_picturePath;
    [SerializeField] string trapMenuItem_borderPath;
    [SerializeField] Sprite unselectedItem;
    [SerializeField] Sprite selectedItem;
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
                UserInterface.Instance.ChangeCursor(cursor_Sell);
            }
            else
            {
                UserInterface.Instance.ChangeCursor(cursor_Build);
            }

            altMode = value;

            TrapMenuRefresh();
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
            if( value >= selectableTraps.Length)
            {
                trapIndex = 0;
            }
            else if(value < 0)
            {
                trapIndex = selectableTraps.Length - 1;
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
            if(value != currentTrap)
            {
                currentTrap = value;
                TrapMenuRefresh();
            }
        }
    }

    #endregion
    #region UnityFunctions

    private void Awake()
    {
        mainCamera = Camera.main;
        trapMenuRect.gameObject.SetActive(false);
        selectableTraps = floorTraps;
    }
    private void Update()
    {
        CheckAltMode();
        CheckHover();
        ChangeSelectedTrap();
        ClickTrap();
    }

    #endregion

    public void Open()
    {
        UserInterface.Instance.ChangeCursor(cursor_Build);
    }
    public void Close()
    {
        UserInterface.Instance.ChangeCursor();
        trapMenuRect.gameObject.SetActive(false);
        CurrentTrap = null;
    }

    private void TrapMenuRefresh()
    {
        if(CurrentTrap != null)
        {
            trapMenuRect.gameObject.SetActive(true);

            if (CurrentTrap.type == Trap.Type.Floor)
            {
                selectableTraps = floorTraps;
            }
            else if (CurrentTrap.type == Trap.Type.Ceil)
            {
                selectableTraps = ceilTraps;
            }

            SetupItemList();

            PositionateTrapMenu();
            DisplayTransaction();
            RefreshSelectedTrap();
        }
        else
        {
            trapMenuRect.gameObject.SetActive(false);
        }
    }
    private void SetupItemList()
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
    private void RefreshSelectedTrap()
    {
        for(int i = 0; i < trapList.childCount; i++)
        {
            if( i == TrapIndex )
            {
                trapList.GetChild(i).GetComponent<Image>().sprite = selectedItem;
            }
            else
            {
                trapList.GetChild(i).GetComponent<Image>().sprite = unselectedItem;
            }
        }
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
    private void DisplayTransaction()
    {
        if (AltMode)
        {
            currentTrap_Image.sprite = CurrentTrap.Data.shopImage;
            selectedTrap_Image.sprite = null;

            arrow_Text.text = CurrentTrap.Data.shopSellingPrice.ToString();
        }
        else
        {
            currentTrap_Image.sprite = CurrentTrap.Data.shopImage;
            selectedTrap_Image.sprite = SelectedTrap.shopImage;

            arrow_Text.text = (CurrentTrap.Data.shopSellingPrice - SelectedTrap.shopPrice).ToString();
        }
    }

    private void ChangeSelectedTrap()
    {
        if(Input.GetButtonDown("NextWeapon"))
        {
            TrapIndex++;
            TrapMenuRefresh();
        }
        else if (Input.GetButtonDown("PreviousWeapon"))
        {
            TrapIndex--;
            TrapMenuRefresh();
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
    private void ClickTrap()
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

            TrapMenuRefresh();
        }

    }

}
