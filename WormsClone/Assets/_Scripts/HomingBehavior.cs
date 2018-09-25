using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBehavior : MonoBehaviour
{
    public string[] taggedToTarget;
    public float targetingRadius;
    public float homingRefreshRate;
    public float rotateSpeed;
    public float speed;

    Rigidbody2D self;
    GameObject target;

    void Start()
    {
        self = GetComponent<Rigidbody2D>();
        CircleCollider2D targetCollider = gameObject.AddComponent<CircleCollider2D>();
        targetCollider.isTrigger = true;
        targetCollider.radius = targetingRadius;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (target == null)
        {
            foreach (string tag in taggedToTarget)
            {
                if (col.gameObject.tag == tag)
                {
                    target = col.gameObject;
                    StartCoroutine(Targeting());
                    break;
                }
            }
        }
    }

    IEnumerator Targeting()
    {
        while (true)
        {
            Vector2 direction = (Vector2)target.transform.position - self.position;

            float rotateAmmount = Vector3.Cross(self.transform.up, direction).z;

            self.angularVelocity = rotateAmmount * rotateSpeed;
            self.velocity = transform.up * speed;
            yield return new WaitForSeconds(homingRefreshRate);
        }
    }
}