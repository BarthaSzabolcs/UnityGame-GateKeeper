using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Stupid_ComingFromSides : MonoBehaviour
{

    #region ShowInEditor
    // Hova haladjon
    public Transform endPos;

    // Milyen gyorsan
    public float speed = 1.0F;
    #endregion

    #region PrivateDolgok
    private Transform startPos;
    #endregion

    void Start()
    {

    }

    void Update()
    {

        // Mozogjunk
        if (this.transform.position.x < endPos.position.x)
        {
            return;
            
        }else
        {
            transform.position = new Vector3(this.transform.position.x - speed, this.transform.position.y, this.transform.position.z);
        }
        
    }
}
