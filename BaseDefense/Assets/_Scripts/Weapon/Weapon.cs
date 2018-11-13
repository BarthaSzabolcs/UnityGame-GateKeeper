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
    [SerializeField] SpriteRenderer weaponSpriteRenderer;
    [SerializeField] SpriteRenderer muzzleflashRenderer;
    [SerializeField] Transform rightHand, leftHand;
    [SerializeField] Transform muzzleFlash;

    [Header("LaserSight:")]
    [SerializeField] LayerMask laserMask;
    [SerializeField] LineRenderer laserRenderer;
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
            if (value > data.Count - 1)
            {
                dataIndex = 0;
            }
            else if (value < 0)
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
                if(WeaponData.reloadAnimation != null)
                {
                    ReloadAnimationCoroutine = StartCoroutine(ReloadAnimation());
                }
            }
            if (reloadingCoroutine != null && value == null)
            {
                StopCoroutine(reloadingCoroutine);
                ReloadAnimationCoroutine = null;
                OnReloadStop?.Invoke();
            }
            reloadingCoroutine = value;
        }
    }

    private Coroutine reloadAnimationCoroutine;
    public Coroutine ReloadAnimationCoroutine
    {
        get
        {
            return reloadAnimationCoroutine;
        }
        set
        {
            if (reloadAnimationCoroutine != null && value == null)
            {
                StopCoroutine(reloadAnimationCoroutine);
            }
            reloadAnimationCoroutine = value;
        }
    }

    private Coroutine firingAnimationCoroutine;
    public Coroutine FiringAnimationCoroutine
    {
        get
        {
            return firingAnimationCoroutine;
        }
        set
        {
            if (firingAnimationCoroutine != null && value == null)
            {
                StopCoroutine(firingAnimationCoroutine);
            }
            firingAnimationCoroutine = value;
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
            else if (value == null && muzzleFlashCoruutine != null)
            {
                StopCoroutine(muzzleFlashCoruutine);
                muzzleflashRenderer.enabled = false;
            }
            muzzleFlashCoruutine = value;
        }
    }

    public int AmmoInMag { get => WeaponInstance.AmmoInMag; }
    public int ExtraAmmo { get => WeaponInstance.ExtraAmmo; }
    public WeaponData WeaponData { get => data[DataIndex]; }
    public WeaponData WeaponInstance { get => instances[DataIndex]; }
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

        OnWeaponChanged?.Invoke(WeaponInstance);
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
        weaponSpriteRenderer.sprite = WeaponData.sprite;
        muzzleflashRenderer.color = WeaponData.bulletData.muzzleFlashColor;
        muzzleflashRenderer.transform.localPosition = WeaponData.muzzleFlashOffSet;

        // Lasersight
        if (data[DataIndex].hasLaserSight)
        {
            laserRenderer.enabled = true;
            laserRenderer.colorGradient = data[DataIndex].laserSightColor;
            laserSightRange = data[DataIndex].LaserSightRange;
        }
        else
        {
            laserRenderer.enabled = false;
        }

        OnWeaponChanged?.Invoke(instances[DataIndex]);
    }

    // Shooting
    public void PullTrigger()
    {
        ReloadCoroutine = null;
        WeaponInstance.PullTrigger();
    }
    public void RelaseTrigger()
    {
        WeaponInstance.ReleaseTrigger();
    }
    public void Reload()
    {
        if (ReloadCoroutine == null)
        {
            ReloadCoroutine = StartCoroutine(WeaponInstance.ReloadRoutine());
        }
    }

    // Animations
    IEnumerator MuzzleFlashRoutine(Sprite[] animation)
    {
        data[DataIndex].muzzleFashAnimation.Randomize(muzzleflashRenderer);

        muzzleflashRenderer.enabled = true;

        foreach (Sprite sprite in animation)
        {
            muzzleflashRenderer.sprite = sprite;
            yield return new WaitForEndOfFrame();
        }

        muzzleflashRenderer.enabled = false;
        MuzzleFlashCoroutine = null;
    }
    IEnumerator ReloadAnimation()
    {
        foreach (Sprite sprite in WeaponData.reloadAnimation.Next())
        {
            weaponSpriteRenderer.sprite = sprite;
            yield return new WaitForEndOfFrame();
        }
        weaponSpriteRenderer.sprite = WeaponData.sprite;
        reloadAnimationCoroutine = null;
    }
    IEnumerator FiringAnimation()
    {
        foreach (Sprite sprite in WeaponData.firingAnimation.Next())
        {
            weaponSpriteRenderer.sprite = sprite;
            yield return new WaitForEndOfFrame();
        }
        weaponSpriteRenderer.sprite = WeaponData.sprite;
        firingAnimationCoroutine = null;
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
            droppedWeaponInstance.data = WeaponInstance;
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
    public void SetApearence(bool flipApperance)
    {
        weaponSpriteRenderer.flipY = flipApperance;

        if (flipApperance)
        {
            transform.localPosition         = new Vector2( -WeaponData.weaponPosition.x,    WeaponData.weaponPosition.y );
            leftHand.localPosition          = new Vector2( WeaponData.leftHandPosition.x,   -WeaponData.leftHandPosition.y );
            rightHand.localPosition         = new Vector2( WeaponData.rightHandPosition.x,  -WeaponData.rightHandPosition.y );
            muzzleFlash.localPosition       = new Vector2( WeaponData.muzzleFlashOffSet.x,  -WeaponData.muzzleFlashOffSet.y );
            laserTransform.localPosition    = new Vector2( WeaponData.laserOffSet.x,        -WeaponData.laserOffSet.y );
        }
        else
        {
            transform.localPosition         = WeaponData.weaponPosition;
            leftHand.localPosition          = WeaponData.leftHandPosition;
            rightHand.localPosition         = WeaponData.rightHandPosition;
            muzzleFlash.localPosition       = WeaponData.muzzleFlashOffSet;
            laserTransform.localPosition    = WeaponData.laserOffSet;
        }
    }

    // Draw lasersight
    private void DrawLaser()
    {
        if(WeaponData.hasLaserSight == false) return;

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
        laserRenderer.SetPositions(linePositions);
    }

    // Accessor for the Weapon class
    public void PlayarFiringAnimations(Sprite[] animation)
    {
        if(MuzzleFlashCoroutine == null)
        {
            MuzzleFlashCoroutine = StartCoroutine(MuzzleFlashRoutine(animation));
        }
        if(FiringAnimationCoroutine == null && WeaponData.firingAnimation != null)
        {
            FiringAnimationCoroutine = (StartCoroutine(FiringAnimation()));
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