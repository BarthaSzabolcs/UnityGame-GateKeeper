using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Sight : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] float viewDistance;
    [SerializeField] float fieldOfView;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] string[] taggedToSight;
    #endregion
    #region HideInEditor
    GameObject target;
    public GameObject Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
            targetCollider = value ? value.GetComponent<Collider2D>() : null;
        }
    }
    Collider2D targetCollider;
    bool targetVisible = false;
    #endregion

    void Start()
    {
        GetComponent<CircleCollider2D>().radius = viewDistance;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (Target == null || targetVisible == false)
        {
           Target = col.gameObject;   
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if(Target != null && targetCollider == col)
        {
            Target = null;
        }
    }
    private void Update()
    {
        if(Target != null)
        {
            //Debug.Log(Target.name);
            if (CheckForLineOfSight(targetCollider) == true)
            {
                targetVisible = true;
            }
            else
            {
                targetVisible = false;
            }
        }
    }

    private bool CheckForEnemyTag(string searchedTag)
    {
        foreach(var tag in taggedToSight)
        {
            if(tag == searchedTag)
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckForLineOfSight(Collider2D col)
    {
        Vector2 direction = col.transform.position - transform.position;

        if (Vector2.Angle(direction, transform.right) < fieldOfView * 0.5f)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, (direction).normalized, viewDistance, obstacleMask);

            UserInterface.Instance.DebugLog("In the field of view.");
            if (hit.collider == col)
            {
                UserInterface.Instance.DebugLog("Can see it.");
                return true;
            }
            return false;
        }
        UserInterface.Instance.DebugLog("Hidden.");
        return false;
    }
}