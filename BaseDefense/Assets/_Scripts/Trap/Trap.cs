using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    #region ShowInEditor

    [Header("Type:")]
    public string type;

    [Header("Data")]
    [SerializeField] private TrapData data;
    [SerializeField] private TrapData nullData;

    #endregion
    #region HideInEditor

    BoxCollider2D triggerZone;
    SpriteRenderer spriteRenderer;
    TrapBehaviour trapBehaviourInstance;

    public TrapData Data
    {
        get
        {
            return data;
        }
        set
        {
            if(value == null)
            {
               data = nullData;
            }
            else
            {
                data = value;
            }

            InitializeTrapData();
        }
    }

    Coroutine recoverCoroutine;
    private Coroutine RecoverCoroutine
    {
        get
        {
            return recoverCoroutine;
        }
        set
        {
            if(recoverCoroutine != null && value == null)
            {
                StopCoroutine(recoverCoroutine);
            }

            recoverCoroutine = value;
        }
    }

    #endregion
    #region UnityFunctions

    void Start()
    {
        triggerZone = GetComponent<BoxCollider2D>();
        triggerZone.size = Data.triggerZoneSize;
        triggerZone.offset = Data.triggerZoneOffset;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Data.sprite;

        GameManager.Instance.OnBuildModeStateChange += HandleBuildModeStateChanged;

        if(Data != nullData)
        {
            InitializeTrapData();
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        foreach (var tag in Data.taggedToDamage)
        {
            if(tag == col.tag)
            {

                trapBehaviourInstance.Trigger();
                return;
            }
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.OnBuildModeStateChange -= HandleBuildModeStateChanged;
    }

    #endregion

    private void InitializeTrapData()
    {

        triggerZone.enabled = data.trapBehaviour.enableTrigger;
        triggerZone.size = data.triggerZoneSize;
        triggerZone.offset = data.triggerZoneOffset;

        spriteRenderer.sprite = GameManager.Instance.InBuildMode ? data.buildModeSprite : data.sprite;

        if (trapBehaviourInstance != null)
        {
            trapBehaviourInstance.CleanUp();
            Destroy(trapBehaviourInstance);
        }

        if (data != nullData)
        {
            trapBehaviourInstance = Instantiate(data.trapBehaviour);
            trapBehaviourInstance.Initialize(transform, triggerZone);
        }

    }
    private void HandleBuildModeStateChanged(bool inBuildMode)
    {
        if(inBuildMode)
        {
            spriteRenderer.sprite = Data.buildModeSprite;
        }
        else
        {
            spriteRenderer.sprite = Data.sprite;
        }
        UserInterface.Instance.DebugLog(inBuildMode.ToString());
    }
    private IEnumerator RecoverRoutine()
    {
        triggerZone.enabled = false;
        yield return new WaitForSeconds(Data.recoverSpeed);

        triggerZone.enabled = true;
        AudioManager.Instance.PlaySound(Data.recoverAudio);

        recoverCoroutine = null;
    }
}
