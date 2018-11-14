using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle_Stupid_Controller : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] AI_Sight sight;
    [SerializeField] float moveForce;
    [SerializeField] float maxSpeed;
    [SerializeField] string targetName;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    float sign = 1;
    Transform target;
    #endregion

    private void Start()
    {
        self = GetComponent<Rigidbody2D>();
        target = GameObject.Find(targetName).transform;
        CheckDirection();
    }
    private void Update()
    {
        if (Mathf.Abs(self.velocity.x) < maxSpeed)
        {
            self.AddForce(sign * Vector2.left * moveForce);
        }
        else
        {
            self.velocity = new Vector2(sign * -maxSpeed, self.velocity.y);
        }
    }
    private void CheckDirection()
    {
        sign = Mathf.Sign(transform.position.x - target.position.x);
    }
}
