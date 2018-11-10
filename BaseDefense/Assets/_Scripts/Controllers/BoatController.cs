using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject GoTo;
    public float speed;

    void Start()
    {
        if (this.transform.position.x < GoTo.transform.position.x)
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // The step size is equal to speed times frame time.
        float step = speed * Time.deltaTime;

        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, GoTo.transform.position, step);

    }


    // For GameManager
    public bool ReachedPos()
    {
        bool reached = false;

        if (this.transform.position.x == GoTo.transform.position.x && this.transform.position.y == GoTo.transform.position.y)
            reached = true;

        return reached;
    }
}
