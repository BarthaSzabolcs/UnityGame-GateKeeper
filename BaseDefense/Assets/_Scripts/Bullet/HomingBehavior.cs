using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBehavior : MonoBehaviour
{
    public string[] taggedToTarget;
    public float targetingRadius;
    public float homingRefreshRate;
    public float targetingRefreshRate = 0.2f;
    public float maxRotationPerCycle;
    public float speed;
    public LayerMask targetMask;

    private Coroutine SearchTargetRoutine
    {
        get
        {
            return searchTarget;
        }
        set
        {
            if(value == null && searchTarget != null)
            {
                StopCoroutine(searchTarget);
            }
            else if (searchTarget != null && value != null)
            {
                StopCoroutine(searchTarget);
            }
            searchTarget = value;
        }
    }
    private Coroutine FollowTargetRoutine
    {
        get
        {
            return followTarget;
        }
        set
        {
            if (value == null && followTarget != null)
            {
                StopCoroutine(followTarget);
            }
            else if (followTarget != null && value != null)
            {
                StopCoroutine(followTarget);
            }
            followTarget = value;
        }
    }
    Coroutine searchTarget;
    Coroutine followTarget;

    Rigidbody2D self;
    GameObject target;
    
    public void Initialize()
    {
        SearchTargetRoutine = null;
        FollowTargetRoutine = null;
        target = null;

        self = GetComponent<Rigidbody2D>();
        SearchTargetRoutine = StartCoroutine(SearchTarget());  
    }

    IEnumerator FollowTarget()
    {
        while (true)
        {
            if (target != null)
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
            //else if (SearchTargetRoutine == null)
            //{
            //    SearchTargetRoutine = StartCoroutine(SearchTarget());
            //}
            yield return new WaitForSeconds(homingRefreshRate);
        }
    }
    IEnumerator SearchTarget()
    {
        while(target == null)
        {
            var potentialTargets = Physics2D.OverlapCircleAll(transform.position, targetingRadius, targetMask);
            foreach(Collider2D col in potentialTargets)
            {
                foreach (string tag in taggedToTarget)
                {
                    if (col.gameObject.tag == tag)
                    {
                        target = col.gameObject;
                        FollowTargetRoutine = StartCoroutine(FollowTarget());
                        SearchTargetRoutine = null;
                    }
                }
            }
            yield return new WaitForSeconds(targetingRefreshRate);
        }
    }
}