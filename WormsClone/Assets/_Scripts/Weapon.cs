﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Events
    public delegate void WeaponChanged(Sprite sprite,WeaponData wdata);
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
    BoxCollider2D damageTrigger;
    public List<WeaponData> instances;
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
    private Coroutine ReloadingRoutine;
    public Coroutine reloadingRoutine { get { return ReloadingRoutine; } set { if (ReloadingRoutine == null) { OnReloadStart(); } if (value == null) { OnReloadStop(); } ReloadingRoutine = value; } }
    public Coroutine meeleRoutine;

    Transform rightHand, leftHand;
    private UserInterface uiManager;
    #endregion

    #region UnityFunctions
    private void Start()
    {
        damageTrigger = GetComponent<BoxCollider2D>();
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

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var meeleData = data[DataIndex] as MeeleWeaponData;
        foreach (var tag in meeleData.taggedToDamage)
        {
            if (tag == coll.gameObject.tag)
            {
                coll.gameObject.GetComponent<Health>().TakeDamage(meeleData.damage, gameObject);
                CancelMeeleAttack();
            }
        }
        
    }
    #endregion
    void Initialize(WeaponData weaponData)
    {
        uiManager = UserInterface.Instance;
        OnWeaponChanged = uiManager.RefreshWeaponSprite;
        OnReloadStart = uiManager.ReloadStart;
        OnReloadStop = uiManager.ReloadStop;
        RangedWeaponData rangedData = weaponData as RangedWeaponData;
        if (rangedData != null)
        {
            rangedData = Instantiate(rangedData);
            rangedData.Initialize
            (
                GetComponent<Rigidbody2D>()
            );
            instances.Add(rangedData);
        }

        MeeleWeaponData meeleData = weaponData as MeeleWeaponData;
        if (meeleData != null)
        {
            meeleData = Instantiate(meeleData);
            meeleData.Initialize
            (
                GetComponent<Rigidbody2D>(),
                damageTrigger,
                wielder
            );
            instances.Add(meeleData);
        }
        OnWeaponChanged(instances[DataIndex].sprite, instances[DataIndex]);
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

        MeeleWeaponData meeleData = data[DataIndex] as MeeleWeaponData;
        if (meeleData != null)
        {
            damageTrigger.size = meeleData.damageTriggerSize;
            damageTrigger.offset = meeleData.damageTriggerOffSet;
        }
        damageTrigger.enabled = false;
        OnWeaponChanged(sRenderer.sprite, instances[DataIndex]);
        
    }

    void Attack()
    {
        var meele = instances[DataIndex] as MeeleWeaponData;
        if (meele != null)
        {
            if (meeleRoutine == null)
            {
                meeleRoutine = StartCoroutine(meele.AttackRoutine());
            }
        }
        else
        {
            if (reloadingRoutine == null)
            {
                instances[dataIndex].Attack();
            }
            else if (reloadingRoutine != null)
            {
                StopCoroutine(reloadingRoutine);
                reloadingRoutine = null;
            }
        }
        

    }
    void CancelMeeleAttack()
    {
        if (meeleRoutine != null) { StopCoroutine(meeleRoutine); };
        meeleRoutine = null;
        damageTrigger.enabled = false;
        wielder.canAim = true;
    }

    void Reload()
    {
        RangedWeaponData ranged = instances[DataIndex] as RangedWeaponData;
        if (ranged != null)
        {
            if (reloadingRoutine == null)
            {
                reloadingRoutine = StartCoroutine(ranged.ReloadRoutine(this));
            }
        }
    }

    void ChangeWeapon(bool next)
    {
        CancelMeeleAttack();
        if (reloadingRoutine != null)
        {
            StopCoroutine(reloadingRoutine);
            reloadingRoutine = null;
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
            droppedWeaponInstance.data = data[DataIndex];
            droppedWeaponInstance.dropDirection = transform.right.normalized;

            var rangedWeapon = instances[DataIndex] as RangedWeaponData;
            if (rangedWeapon)
            {
                droppedWeaponInstance.ammoInMag = rangedWeapon.ammoInMag;
                droppedWeaponInstance.extraAmmo = rangedWeapon.extraAmmo;
            }

            instances.RemoveAt(DataIndex);
            data.RemoveAt(DataIndex);
            RefreshData();
        }
    }
    public void PickUpWeapon(WeaponData newWeaponData)
    {
        data.Add(newWeaponData);
        //instances.Add(newWeaponData);
        Initialize(newWeaponData);

        //var newRangedWeaponData = newWeaponData as RangedWeaponData;
        //if (newRangedWeaponData)
        //{
        //    var rangedWeaponInstance = instances[instances.Count - 1] as RangedWeaponData;

        //    rangedWeaponInstance.ammoInMag = newRangedWeaponData.ammoInMag;
        //    rangedWeaponInstance.extraAmmo = newRangedWeaponData.extraAmmo;
        //}
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
