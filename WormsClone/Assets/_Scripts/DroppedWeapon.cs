using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] float pickUpDelay;
    #endregion
    #region HideInEditor
    [HideInInspector] public WeaponData data;
    [HideInInspector] public Vector2 dropDirection;
    SpriteRenderer Srenderer;
    BoxCollider2D trigger;
    #endregion

    #region UnityFunctions
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        StartCoroutine(AddCollider());
        GetComponent<Rigidbody2D>().velocity = dropDirection * 20;
    }
    #endregion
    IEnumerator AddCollider()
    {
        yield return new  WaitForSeconds(pickUpDelay);

        trigger = gameObject.AddComponent<BoxCollider2D>();
        float x = data.sprite.bounds.size.x;
        float y = data.sprite.bounds.size.y;
        Vector2 size = new Vector2(x,y);
        trigger.size = size;
    }
}
