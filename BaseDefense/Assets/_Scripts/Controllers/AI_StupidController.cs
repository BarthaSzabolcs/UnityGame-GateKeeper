using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StupidController : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] AI_Sight sight;
    [SerializeField] float moveForce;
    [SerializeField] float maxSpeed;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    float sign = 1;
    #endregion

    private void Start()
    {
        self = GetComponent<Rigidbody2D>();
        sign = Mathf.Sign(transform.position.x);

    }
    private void Update()
    {
        if (sight.TargetVisible == false && Mathf.Abs(self.velocity.x) < maxSpeed)
        {
            self.AddForce(sign * Vector2.left * moveForce);
        }
        else if(sight.TargetVisible == false)
        {
            self.velocity = new Vector2(sign * -maxSpeed, self.velocity.y);
        }
    }
}