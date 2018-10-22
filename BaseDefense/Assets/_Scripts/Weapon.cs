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
    public delegate void MagChange(int inMag);
    public event MagChange OnMagChange;
    public delegate void ExtraAmmoChange(int extraMag);
    public event ExtraAmmoChange OnExtraAmmoChange;
    public delegate void ReloadChange(float time, float percent);
    public event ReloadChange OnReloadChange;
    #endregion
    #region ShowInEditor
    [SerializeField] List<WeaponData> data;
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
                OnReloadStart?.Invoke();
            }
            if (value == null)
            {
                OnReloadStop?.Invoke();
            }
            reloadingRoutine = value;
        }
    }
    Transform rightHand, leftHand;
    #endregion

    #region UnityFunctions
    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();

        rightHand = transform.Find("RightHand");
        leftHand = transform.Find("LeftHand");

        InitializeInstances();
        RefreshData();
    }

    #endregion
    void Initialize(WeaponData weaponData)
    {
        var instance = Instantiate(weaponData);
        instance.Initialize
        (
            GetComponent<Rigidbody2D>(),
            this
        );
        instances.Add(instance);

        OnWeaponChanged?.Invoke(instances[DataIndex]);
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
        //wielder.weaponIsAuto = data[DataIndex].isAuto;

        OnWeaponChanged?.Invoke(instances[DataIndex]);
    }

    public void Attack()
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
    public void Reload()
    {
        if (ReloadRoutine == null)
        {
            ReloadRoutine = StartCoroutine(instances[DataIndex].ReloadRoutine());
        }
    }

    public void ChangeWeapon(bool next)
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
    public void DropWeapon()
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
    #region EventTriggerFromData
    public void TriggerMagChange(int inMag)
    {
        OnMagChange?.Invoke(inMag);
    }
    public void TriggerExtraAmmoChange(int extraMag)
    {
        OnExtraAmmoChange?.Invoke(extraMag);
    }
    public void TriggerReloadChange(float time, float percent)
    {
        OnReloadChange?.Invoke(time, percent);
    }
    #endregion
}
