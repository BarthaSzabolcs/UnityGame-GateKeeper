using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [Header("LifeTime:")]
    public float lifeTime;

    [Header("Explosion:")]
    public string explosionAudio;
    public GameObject explosionObject;
    public ExplosionData explosionData;

    private Coroutine selfDestructRoutine;

    public void StartSelfDestruct()
    {
        selfDestructRoutine = StartCoroutine(SelfDesruct());
    }
    public void StopSelfDestruct()
    {
        if(selfDestructRoutine != null)
        {
            StopCoroutine(selfDestructRoutine);
        }
    }
    IEnumerator SelfDesruct()
    {
        yield return new WaitForSeconds(lifeTime);
        Explode();
    }
    void Explode()
    {
        AudioManager.Instance.PlaySound(explosionAudio);
        if (explosionObject != null && explosionData != null)
        {
            var explosion = Instantiate(explosionObject, transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().data = explosionData;
        }
        ObjectPool.Instance.Pool(ObjectPool.Types.bullet, gameObject);
    }
}
