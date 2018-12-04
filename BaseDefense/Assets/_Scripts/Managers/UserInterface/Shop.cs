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

    [Header("GunShopItem Components:")]
    [SerializeField] RectTransform gunShopRect;
    [SerializeField] GameObject gunShopItem;
    [SerializeField] string buyWeaponButton_path;
    [SerializeField] string weaponPrice_path;

    [SerializeField] string buyAmmoButton_path;
    [SerializeField] string ammoPrice_path;

    [SerializeField] string weaponImage_path;

    [Header("TrapShopItem Components:")]
    [SerializeField] GameObject trapMenuItem;
    [SerializeField] string trapMenuItem_picturePath;
    [SerializeField] Sprite Sprite_nullData;
    [SerializeField] Sprite Sprite_unselected;
    [SerializeField] Sprite Sprite_selected;
    [SerializeField] RectTransform trapList;

    [SerializeField] RectTransform trapMenuRect;
    [SerializeField] LayerMask clickLayer;

    [SerializeField] Image currentTrap_Image;
    [SerializeField] Image selectedTrap_Image;
    [SerializeField] Text arrow_Text;

    #endregion
    #region Hide In Editor

    public enum ShopType { Trap, Gun};
    private ShopType currentShopType;

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
            if (value >= selectableTraps.Length)
            {
                trapIndex = 0;
            }
            else if (value < 0)
            {
                trapIndex = selectableTraps.Length - 1;
            }
            else
            {
                trapIndex = value;
            }

            RefreshSelectedTrap();
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
            if (value != currentTrap)
            {
                if (value?.type == Trap.Type.Floor)
                {
                    selectableTraps = floorTraps;
                    SetupTrapShop();
                }
                else if (value?.type == Trap.Type.Ceil)
                {
                    selectableTraps = ceilTraps;
                    SetupTrapShop();
                }

                currentTrap = value;

                TrapMenuRefresh();
            }
        }
    }

    List<WeaponData> carriedWeapons;

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

    public void Open(ShopType type)
    {
        currentShopType = type;
        
        if(currentShopType == ShopType.Trap)
        {
            UserInterface.Instance.ChangeCursor(cursor_Build);
        }
        else if (currentShopType == ShopType.Gun)
        {
            gunShopRect.gameObject.SetActive(true);
            SetupGunShop();
        }
    }
    public void Close()
    {
        gunShopRect.gameObject.SetActive(false);
        UserInterface.Instance.ChangeCursor();
        trapMenuRect.gameObject.SetActive(false);
        CurrentTrap = null;
    }

    // TrapShop
    private void TrapMenuRefresh()
    {
        if (CurrentTrap != null)
        {
            trapMenuRect.gameObject.SetActive(true);

            PositionateTrapMenu();

            DisplayTrapTransaction();
        }
        else
        {
            trapMenuRect.gameObject.SetActive(false);
        }
    }
    private void SetupTrapShop()
    {
        for (int i = trapList.childCount - 1; i >= 0; i--)
        {
            Destroy(trapList.GetChild(i).gameObject);
        }

        trapList.DetachChildren();

        foreach (TrapData trap in selectableTraps)
        {
            AddTrapToShop(trap);
        }

        TrapIndex = trapIndex;
    }
    private void AddTrapToShop(TrapData trapData)
    {
        var trap = Instantiate(trapMenuItem, Vector2.zero, Quaternion.identity);

        trap.transform.SetParent(trapList, false);
        trap.transform.Find(trapMenuItem_picturePath).GetComponent<Image>().sprite = trapData.shopImage;
    }
    private void RefreshSelectedTrap()
    {
        for (int i = 0; i < trapList.childCount; i++)
        {
            if (i == TrapIndex)
            {
                trapList.GetChild(i).GetComponent<Image>().sprite = Sprite_selected;
            }
            else
            {
                trapList.GetChild(i).GetComponent<Image>().sprite = Sprite_unselected;
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
    private void DisplayTrapTransaction()
    {
        if (AltMode)
        {
            currentTrap_Image.sprite = CurrentTrap.Data.shopImage;
            selectedTrap_Image.sprite = Sprite_nullData;

            arrow_Text.text = CurrentTrap.Data.shopSellingPrice.ToString();
        }
        else
        {
            if (CurrentTrap.Data != SelectedTrap)
            {
                currentTrap_Image.sprite = CurrentTrap.Data.shopImage;
                selectedTrap_Image.sprite = SelectedTrap.shopImage;

                arrow_Text.text = (CurrentTrap.Data.shopSellingPrice - SelectedTrap.shopPrice).ToString();
            }
            else
            {
                currentTrap_Image.sprite = CurrentTrap.Data.shopImage;
                selectedTrap_Image.sprite = SelectedTrap.shopImage;

                arrow_Text.text = "0";
            }
        }
    }

    private void ChangeSelectedTrap()
    {
        if (Input.GetButtonDown("NextWeapon"))
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

                GameManager.Instance.Money += CurrentTrap.Data.shopSellingPrice;
                CurrentTrap.Data = null;

            }
            else if ((GameManager.Instance.Money - SelectedTrap.shopPrice >= 0) && (CurrentTrap.Data != SelectedTrap))
            {

                GameManager.Instance.Money += CurrentTrap.Data.shopSellingPrice;
                GameManager.Instance.Money -= SelectedTrap.shopPrice;

                CurrentTrap.Data = SelectedTrap;
            }

            TrapMenuRefresh();
        }

    }

    // GunShop
    private void CloseTrapShop()
    {
        gunShopRect.gameObject.SetActive(false);
    }

    public void AddWeaponToShop(WeaponData weaponData)
    {
        bool weaponCarried = false;
        foreach (var weapon in carriedWeapons)
        {
            if (weaponData.shopName == weapon.shopName)
            {
                weaponCarried = true;
                break;
            }
        }

        var item = Instantiate(gunShopItem, Vector2.zero, Quaternion.identity);
        item.transform.SetParent(gunShopRect, false);

        item.transform.Find(weaponImage_path).GetComponent<Image>().sprite = weaponData.shopSprite;

        if (weaponCarried == false)
        {
            item.transform.Find(buyWeaponButton_path).GetComponent<Button>().onClick.AddListener(() => BuyWeapon(weaponData));
            item.transform.Find(weaponPrice_path).GetComponent<Text>().text = weaponData.shopPrice.ToString();
        }
        else
        {
            item.transform.Find(buyWeaponButton_path).gameObject.SetActive(false);
            item.transform.Find(buyAmmoButton_path).GetComponent<Button>().onClick.AddListener(() => BuyAmmo(weaponData));
            item.transform.Find(ammoPrice_path).GetComponent<Text>().text = weaponData.shopAmmoPrice.ToString();
        }

    }
    private void SetupGunShop()
    {
        carriedWeapons = GameManager.Instance.Player.GetComponent<PlayerController>().CarriedWeapon.Weapons;

        for (int i = gunShopRect.childCount - 1; i >= 0; i--)
        {
            Destroy(gunShopRect.GetChild(i).gameObject);
        }

        gunShopRect.DetachChildren();

        foreach (WeaponData weapon in weapons)
        {
            AddWeaponToShop(weapon);
        }
    }
    private void BuyWeapon(WeaponData weaponData)
    {
        if (GameManager.Instance.Money >= weaponData.shopPrice)
        {
            GameManager.Instance.Player.GetComponent<PlayerController>().CarriedWeapon.AddWeapon(weaponData);
            SetupGunShop();
            GameManager.Instance.Money -= weaponData.shopPrice;
        }
    }
    private void BuyAmmo(WeaponData weaponData)
    {
        if(GameManager.Instance.Money >= weaponData.shopAmmoPrice)
        {
            GameManager.Instance.Player.GetComponent<PlayerController>().CarriedWeapon.AddAmmo(weaponData.shopName, weaponData.shopMagSize);
            GameManager.Instance.Money -= weaponData.shopAmmoPrice;
        }
    }

}