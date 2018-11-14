using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    [SerializeField] float pickUpDelay;
    public WeaponData data;
    public Vector2 dropDirection;

    public int ammoInMag;
    public int extraAmmo;

    SpriteRenderer Srenderer;
    BoxCollider2D trigger;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        StartCoroutine(AddCollider());
        GetComponent<Rigidbody2D>().velocity = dropDirection * 20;
    }

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
