using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] private TrapData data;
    [SerializeField] private TrapData nullData;
    #endregion
    #region HideInEditor
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
            triggerZone.size = data.triggerZoneSize;
            triggerZone.offset = data.triggerZoneOffset;
            spriteRenderer.sprite = GameManager.Instance.InBuildMode ? data.buildModeSprite : data.sprite;   
        }
    }
    BoxCollider2D triggerZone;
    SpriteRenderer spriteRenderer;
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
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        foreach (var tag in Data.taggedToDamage)
        {
            if(tag == col.tag)
            {
                col.gameObject.GetComponent<Health>().TakeDamage(Data.damage);
            }
        }
        StartCoroutine(RecoverRoutine());
    }
    private void OnDisable()
    {
        GameManager.Instance.OnBuildModeStateChange -= HandleBuildModeStateChanged;
    }
    #endregion
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
    }
}
