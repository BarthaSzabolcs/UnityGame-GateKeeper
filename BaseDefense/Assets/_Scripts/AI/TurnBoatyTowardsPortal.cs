using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBoatyTowardsPortal : MonoBehaviour
{
    public GameObject turnTowards;
    // Start is called before the first frame update
    void Start()
    {
        if (this.transform.position.x < turnTowards.transform.position.x)
        {

        }
        else
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
