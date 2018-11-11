using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Events
    public delegate void WeaponChanged(WeaponData wdata);
    public event WeaponChanged OnWeaponChanged;

    public delegate void ReloadStateChange();
    public event ReloadStateChange OnReloadStart;
    public event ReloadStateChange OnReloadStop;

    public delegate void AmmoChange(int inMag);
    public event AmmoChange OnMagChange;
    public event AmmoChange OnExtraAmmoChange;

    public delegate void MagEmpty();
    public event MagEmpty OnMagEmpty;
    
    public delegate void ReloadChange(float time, float percent);
    public event ReloadChange OnReloadChange;
    #endregion
    #region ShowInEditor

    [Header("Weapons:")]
    [SerializeField] List<WeaponData> data;
    [SerializeField] GameObject droppedWeapon;

    [Header("Components:")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer muzzleflashRenderer;
    [SerializeField] Transform rightHand, leftHand;
    [SerializeField] Transform muzzleFlash;

    [Header("LaserSight:")]
    [SerializeField] LayerMask laserMask;
    [SerializeField] LineRenderer laserSight;
    [SerializeField] Transform laserTransform;
    #endregion
    #region HideInEditor

    // LineRenderer
    Vector3[] linePositions = new Vector3[2];
    float laserSightRange;

    // Components
    List<WeaponData> instances;

    // Properties
    int dataIndex = 0;
    private int DataIndex
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
    private Coroutine reloadingCoroutine;
    public Coroutine ReloadCoroutine
    {
        get
        {
            return reloadingCoroutine;
        }
        set
        {
            if (reloadingCoroutine == null && value != null)
            {
                OnReloadStart?.Invoke();
            }
            if (reloadingCoroutine != null && value == null)
            {
                StopCoroutine(reloadingCoroutine);
                OnReloadStop?.Invoke();
            }
            reloadingCoroutine = value;
        }
    }
    private Coroutine muzzleFlashCoruutine;
    private Coroutine MuzzleFlashCoroutine
    {
        get
        {
            return muzzleFlashCoruutine;
        }
        set
        {
            if (value != null && muzzleFlashCoruutine != null)
            {
                StopCoroutine(muzzleFlashCoruutine);
            }
            else if(value == null && muzzleFlashCoruutine != null)
            {
                StopCoroutine(muzzleFlashCoruutine);
                muzzleflashRenderer.enabled = false;
            }
            muzzleFlashCoruutine = value;
        }
    }
    public int AmmoInMag { get => instances[DataIndex].AmmoInMag; }
    public int ExtraAmmo { get => instances[DataIndex].ExtraAmmo; }
    public bool IsAuto { get => instances[DataIndex].isAuto; }
    
    #endregion
    #region UnityFunctions

    private void Awake()
    {
        InitializeInstances();
        RefreshData();
    }
    private void Update()
    {
        DrawLaser();
    }

    #endregion
    #region CustomFunctions

    // Initialization
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
        spriteRenderer.sprite = data[DataIndex].sprite;
        muzzleflashRenderer.color = data[DataIndex].bulletData.muzzleFlashColor;
        muzzleflashRenderer.transform.localPosition = data[DataIndex].muzzleFlashOffSet;

        // Lasersight
        if (data[DataIndex].hasLaserSight)
        {
            laserSight.enabled = true;
            laserSight.colorGradient = data[DataIndex].laserSightColor;
            laserSightRange = data[DataIndex].LaserSightRange;
        }
        else
        {
            laserSight.enabled = false;
        }

        OnWeaponChanged?.Invoke(instances[DataIndex]);
    }

    // Shooting
    public void PullTrigger()
    {
        ReloadCoroutine = null;
        instances[dataIndex].PullTrigger();
    }
    public void RelaseTrigger()
    {
        instances[dataIndex].ReleaseTrigger();
    }
    IEnumerator MuzzleFlashRoutine(Sprite[] animation)
    {
        data[DataIndex].muzzleFashAnimation.Randomize(spriteRenderer);

        muzzleflashRenderer.enabled = true;

        foreach (Sprite sprite in animation)
        {
            muzzleflashRenderer.sprite = sprite;
            yield return new WaitForEndOfFrame();
        }

        muzzleflashRenderer.enabled = false;
        MuzzleFlashCoroutine = null;
    }
    public void Reload()
    {
        if (ReloadCoroutine == null)
        {
            ReloadCoroutine = StartCoroutine(instances[DataIndex].ReloadRoutine());
        }
    }

    // WeaponData Handling
    public void ChangeWeapon(bool next)
    {
        ReloadCoroutine = null;
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
    public void AddWeapon(WeaponData newWeaponData)
    {
        data.Add(newWeaponData);
        Initialize(newWeaponData);
    }
    public void SetApearence(bool lookingRight)
    {
        if (lookingRight)
        {
            spriteRenderer.flipY = false;
            transform.localPosition = data[DataIndex].weaponPosition;
            leftHand.localPosition = data[DataIndex].leftHandPosition;
            rightHand.localPosition = data[DataIndex].rightHandPosition;
            muzzleflashRenderer.transform.localPosition = data[DataIndex].muzzleFlashOffSet;
            laserTransform.localPosition = data[DataIndex].laserOffSet; 
        }
        else
        {
            spriteRenderer.flipY = true;
            transform.localPosition = new Vector2(-data[DataIndex].weaponPosition.x, data[DataIndex].weaponPosition.y);
            leftHand.localPosition  = new Vector2(data[DataIndex].leftHandPosition.x, -data[DataIndex].leftHandPosition.y);
            rightHand.localPosition = new Vector2(data[DataIndex].rightHandPosition.x, -data[DataIndex].rightHandPosition.y);
            muzzleFlash.localPosition = new Vector2(data[DataIndex].muzzleFlashOffSet.x, -data[DataIndex].muzzleFlashOffSet.y);
            laserTransform.localPosition = new Vector2(data[DataIndex].laserOffSet.x, -data[DataIndex].laserOffSet.y);
        }
    }

    // Draw lasersight
    private void DrawLaser()
    {
        if(data[DataIndex].hasLaserSight == false) return;

        RaycastHit2D hit = Physics2D.Raycast(laserTransform.position, transform.right, laserSightRange, laserMask, 0);
        linePositions[0] = laserTransform.position;
        if (hit.collider != null)
        {
            linePositions[1] = hit.point;
        }
        else
        {
            linePositions[1] = transform.TransformPoint(Vector2.right * laserSightRange);
        }
        laserSight.SetPositions(linePositions);
    }

    // Accessor for the Weapon class
    public void MuzleFlash(Sprite[] animation)
    {
        if(MuzzleFlashCoroutine == null)
        {
            MuzzleFlashCoroutine = StartCoroutine(MuzzleFlashRoutine(animation));
        }
    }
    public void TriggerMagChange(int inMag)
    {
        OnMagChange?.Invoke(inMag);
        if(inMag == 0)
        {
            OnMagEmpty?.Invoke();
        }
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