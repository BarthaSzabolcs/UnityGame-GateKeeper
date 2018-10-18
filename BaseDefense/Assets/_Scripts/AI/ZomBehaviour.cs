using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomBehaviour : MonoBehaviour
{
    #region ShowInEditor
    Collider2D attackTrigger;
    Rigidbody2D self;
    float moveSpeed;
    #endregion

    private void Start()
    {
        self = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        self.velocity = Vector2.right * moveSpeed;
    }
}
