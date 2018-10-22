using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Sight : MonoBehaviour
{
    #region Events
    public delegate void TargetFound();
    /// <summary>
    /// Fired when there is a new target or the current target become visible again.
    /// </summary>
    public event TargetFound OnTargetFound;

    public delegate void TargetLost();
    /// <summary>
    /// Fired if lost line of sight of the target.
    /// </summary>
    public event TargetLost OnTargetLost;
    #endregion
    #region ShowInEditor
    [SerializeField] float viewDistance;
    [SerializeField]
    [Range(0.0f, 360f)] float fieldOfView;
    [Header("Line Of Sight:")]
    [SerializeField] bool checkLineOfSight;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] string[] taggedToSight;
    #endregion
    #region HideInEditor
    GameObject target;
    bool targetVisible = false;

    public GameObject Target
    {
        get
        {
            return target;
        }
        private set
        {
            target = value;
            if (value == null)
            {
                targetCollider = null;
            }
            else
            {
                targetCollider = target.GetComponent<Collider2D>();
            }
        }
    }
    public bool TargetVisible
    {
        get
        {
            return targetVisible;
        }
        private set
        {
            if(targetVisible && value == false)
            {
                OnTargetLost?.Invoke();
            }
            else if (targetVisible == false && value == true)
            {
                OnTargetFound?.Invoke();
            }
            targetVisible = value;
        }
    }
    public float LineOfSightAngle { get; private set; }
    Collider2D targetCollider;
    #endregion

    void Start()
    {
        GetComponent<CircleCollider2D>().radius = viewDistance;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (Target == null || TargetVisible == false)
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
            TargetVisible = CheckForLineOfSight(targetCollider);
        }
        else
        {
            TargetVisible = false;
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
        if (checkLineOfSight == false) return true;

        Vector2 direction = col.transform.position - transform.position;
        LineOfSightAngle = Vector2.SignedAngle(transform.right, direction);

        if (Mathf.Abs(LineOfSightAngle) < fieldOfView * 0.5f)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, (direction).normalized, viewDistance, obstacleMask);

            if (hit.collider == col)
            {
                return true;
            }
        }
        return false;
    }
}