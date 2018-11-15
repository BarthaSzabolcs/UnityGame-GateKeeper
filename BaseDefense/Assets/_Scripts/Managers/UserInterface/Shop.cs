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
    [SerializeField] RectTransform trapIconRect;
    [SerializeField] LayerMask clickLayer;
    [SerializeField] Image selectedTrap_Image;

    #endregion
    #region Hide In Editor
    
    bool altMode;
    Camera mainCamera;

    private int selectedItem;
    private TrapData SelectedItem
    {
        get
        {
            return ceilTraps[selectedItem];
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
        TrapClick();
        ChangeSelectedTrap();
        // CheckHover();

    }
    private void LateUpdate()
    {

        SetTrapIconPosition();

    }
    #endregion

    public void OpenShop()
    {

        //foreach (TrapData trap in floorTraps)
        //{
        //    AddTrapToItems(trap);
        //}

        UserInterface.Instance.ChangeCursor(cursor_Build);
        
    }
    public void CloseShop()
    {

        //for (int i = 0; i < trapList.childCount; i++)
        //{
        //    Destroy(trapList.GetChild(i).gameObject);
        //}

    }

    //private void AddTrapToItems(TrapData trapData)
    //{

    //    var trap = Instantiate(trapMenuItem, Vector2.zero, Quaternion.identity);

    //    trap.transform.SetParent(trapList, false);
    //    trap.transform.Find(trapMenuItem_picturePath).GetComponent<Image>().sprite = trapData.shopImage;

    //}

    private void SetTrapIconPosition()
    {

        trapIconRect.position = Input.mousePosition + new Vector3(25, 25, 0);

    }
    private void RefreshDisplayedData(TrapData trapData)
    {

        selectedTrap_Image.sprite = trapData.shopImage;

    }

    private void ChangeSelectedTrap()
    {

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {

            if(selectedItem  +1 > ceilTraps.Length -1)
            {
                selectedItem = 0;
            }
            else
            {
                selectedItem++;
            }

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            
            if (selectedItem - 1 < 0)
            {
                selectedItem = ceilTraps.Length - 1;
            }
            else
            {
                selectedItem--;
            }

        }

        RefreshDisplayedData(SelectedItem);

    }
    private void CheckAltMode()
    {

        if (Input.GetButtonDown("BuildMode_Shift"))
        {

            altMode = true;
            UserInterface.Instance.ChangeCursor(cursor_Sell);

        }
        else if (Input.GetButtonUp("BuildMode_Shift"))
        {

            altMode = false;
            UserInterface.Instance.ChangeCursor(cursor_Build);

        }

    }
    private void CheckHover()
    {

        if (altMode)
        {
            Ray ray = mainCamera.ViewportPointToRay(mainCamera.ScreenToViewportPoint(Input.mousePosition));

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 30f, clickLayer);

            if (hit.collider != null)
            {

                // trapUnderMouse = hit.collider.gameObject.GetComponent<Trap>();
                // Show the selling price of the trap

            }

        }
        
    }
    private void TrapClick()
    {

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = mainCamera.ViewportPointToRay(mainCamera.ScreenToViewportPoint(Input.mousePosition));

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 30f, clickLayer);

            if (hit.collider != null)
            {

                var trapUnderMouse = hit.collider.gameObject.GetComponent<Trap>();

                if (altMode)
                {

                    GameManager.Instance.Money += trapUnderMouse.Data.shopSellingPrice;
                    trapUnderMouse.Data = null;

                }
                else if(GameManager.Instance.Money - SelectedItem.shopPrice >= 0)
                {

                    GameManager.Instance.Money += trapUnderMouse.Data.shopSellingPrice;
                    GameManager.Instance.Money -= SelectedItem.shopPrice;

                    trapUnderMouse.Data = SelectedItem;

                }

            }

        }

    }

}
