using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new", menuName = "Data / TrapBehaviour_Spawner")]
public class TrapBehaviour_Spawn : TrapBehaviour
{
    #region Show In Editor

    [Header("Settings:")]
    [SerializeField] GameObject[] actuators;
    [SerializeField] Vector2[] actuatorPositions;

    #endregion
    #region Hide In Editor

    private Transform transform;
    private  Collider2D triggerCollider;

    #endregion

    public override void Initialize(Transform transform, Collider2D triggerCollider)
    {

        this.triggerCollider = triggerCollider;
        this.transform = transform;

        for (int i = 0; i < actuators.Length; i++)
        {
            Instantiate(actuators[i], (Vector2)transform.position + actuatorPositions[i], Quaternion.identity, transform);
        }

    }
    public override void CleanUp()
    {

        for(int i = 0; i < actuators.Length; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

    }
}
