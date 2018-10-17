using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float lifeTime;

    private void Start()
    {
        StartCoroutine(SelfDesruct());
    }
    IEnumerator SelfDesruct()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
