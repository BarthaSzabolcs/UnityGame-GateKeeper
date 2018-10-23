using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBehavior : MonoBehaviour
{
    public string[] taggedToTarget;
    public float targetingRadius;
    public float homingRefreshRate;
    public float maxRotationPerCycle;
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
            if(target != null)
            {
                float signedAngle = Vector2.SignedAngle(transform.up, target.transform.position - transform.position);

                float rotation =
                    Mathf.Abs(signedAngle) < maxRotationPerCycle ?
                    signedAngle :
                    Mathf.Sign(signedAngle) * maxRotationPerCycle;

                Vector3 rotationVector = new Vector3(0, 0, rotation);
                transform.Rotate(rotationVector);

                self.velocity = transform.up * speed;
            }
            yield return new WaitForSeconds(homingRefreshRate);
        }
    }
}