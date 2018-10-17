using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Events
    public delegate void WeaponChanged(WeaponData wdata);
    public event WeaponChanged OnWeaponChanged;
    public delegate void ReloadStart();
    public delegate void ReloadStop();
    public event ReloadStart OnReloadStart;
    public event ReloadStop OnReloadStop;
    #endregion
    #region ShowInEditor
    [SerializeField] List<WeaponData> data;
    [SerializeField] PlayerController wielder;
    [SerializeField] GameObject droppedWeapon;
    #endregion
    #region HideInEditor
    SpriteRenderer sRenderer;
    List<WeaponData> instances;
    int dataIndex = 0;
    public int DataIndex
    {
        get
        {
            if (dataIndex < data.Count)
            {
                return dataIndex;
            }
            else
            {
                DataIndex = dataIndex;
                return dataIndex;
            }

        }
        set
        {
            if(value > data.Count - 1)
            {
                dataIndex = 0;
            }
            else if(value < 0)
            {
                dataIndex = data.Count - 1;
            }
            else
            {
                dataIndex = value;
            }
        }
    }
    private Coroutine reloadingRoutine;
    public Coroutine ReloadRoutine
    {
        get
        {
            return reloadingRoutine;
        }
        set
        {
            if (reloadingRoutine == null)
            {
                if(OnReloadStart != null) OnReloadStart();
            }
            if (value == null)
            {
                if(OnReloadStop != null) OnReloadStop();
            }
            reloadingRoutine = value;
        }
    }
    Transform rightHand, leftHand;
    #endregion

    #region UnityFunctions
    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();

        rightHand = transform.Find("RightHand");
        leftHand = transform.Find("LeftHand");

        wielder.OnAttackTriggered += Attack;
        wielder.OnReloadTriggered += Reload;
        wielder.OnWeaponChangeTriggered += ChangeWeapon;
        wielder.OnWeaponDropTriggered += DropWeapon;

        InitializeInstances();
        RefreshData();
    }

    #endregion
    void Initialize(WeaponData weaponData)
    {
        //OnWeaponChanged = UserInterface.Instance.RefreshWeaponData;
        //OnReloadStart = UserInterface.Instance.ReloadStart;
        //OnReloadStop = UserInterface.Instance.ReloadStop;
        var instance = Instantiate(weaponData);
        instance.Initialize
        (
            GetComponent<Rigidbody2D>(),
            this
        );
        instances.Add(instance);

        if(OnWeaponChanged != null)
        {
            OnWeaponChanged(instances[DataIndex]);
        }
    }
    void InitializeInstances()
    {
        instances = new List<WeaponData>();
        foreach (var weaponData in data)
        {
            Initialize(weaponData);
        }
    }
    void RefreshData()
    {
        sRenderer.sprite = data[DataIndex].sprite;
        wielder.weaponIsAuto = data[DataIndex].isAuto;

        if(OnWeaponChanged != null)
        {
            OnWeaponChanged(instances[DataIndex]);
        }
    }

    void Attack()
    {
        if (ReloadRoutine == null)
        {
            instances[dataIndex].Attack();
        }
        else if (ReloadRoutine != null)
        {
            StopCoroutine(ReloadRoutine);
            ReloadRoutine = null;
            instances[dataIndex].Attack();
        }
    }
    void Reload()
    {
        if (ReloadRoutine == null)
        {
            ReloadRoutine = StartCoroutine(instances[DataIndex].ReloadRoutine());
        }
    }

    void ChangeWeapon(bool next)
    {
        if (ReloadRoutine != null)
        {
            StopCoroutine(ReloadRoutine);
            ReloadRoutine = null;
        }
        if(next)
        {
            DataIndex++;
        }
        else
        {
            DataIndex--;
        }
        RefreshData();
    }
    void DropWeapon()
    {
        if (instances.Count > 1)
        {
            GameObject weapon = Instantiate(droppedWeapon, transform.position, transform.rotation);
            DroppedWeapon droppedWeaponInstance = weapon.GetComponent<DroppedWeapon>();
            droppedWeaponInstance.data = instances[DataIndex];
            droppedWeaponInstance.dropDirection = transform.right.normalized;

            instances.RemoveAt(DataIndex);
            data.RemoveAt(DataIndex);
            RefreshData();
        }
    }
    public void PickUpWeapon(WeaponData newWeaponData)
    {
        data.Add(newWeaponData);
        Initialize(newWeaponData);
    }
    #region Appearence
    public void SetApearence(bool lookingRight)
    {
        if (lookingRight)
        {
            sRenderer.flipY = false;
            transform.localPosition = data[DataIndex].weaponPosition;
            leftHand.localPosition = data[DataIndex].leftHandPosition;
            rightHand.localPosition = data[DataIndex].rightHandPosition;
        }
        else
        {
            sRenderer.flipY = true;
            transform.localPosition = new Vector2(-data[DataIndex].weaponPosition.x, data[DataIndex].weaponPosition.y);
            leftHand.localPosition  = new Vector2(data[DataIndex].leftHandPosition.x, -data[DataIndex].leftHandPosition.y);
            rightHand.localPosition = new Vector2(data[DataIndex].rightHandPosition.x, -data[DataIndex].rightHandPosition.y);
        }
    }
    #endregion
}
