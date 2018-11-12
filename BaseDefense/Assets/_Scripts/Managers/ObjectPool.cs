 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	#region ShowInEditor
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private GameObject explosionPrefab;
    #endregion
    #region HideInEditor
    public enum Types{ bullet, explosion};
    public static ObjectPool Instance { get; private set; }
	private List<GameObject> bulletPool = new List<GameObject>();
	private List<GameObject> explosionPool = new List<GameObject>();
    #endregion

    #region UnityFunctions

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion
    #region CustomFunctions

    public void Pool(Types type, GameObject obj)
    {
        obj.SetActive(false);
        if (type == Types.bullet)
        {
            bulletPool.Add(obj);
        }
        else if (type == Types.explosion)
        {
            explosionPool.Add(obj);
        }
    }
    public GameObject Spawn(Types type, Vector2 position)
    {
        if(type == Types.bullet)
        { 
            if (bulletPool.Count > 0)
            {
                GameObject bullet = bulletPool[0];
                bulletPool.RemoveAt(0);
                bullet.transform.position = position;
                bullet.SetActive(true);
                bullet.transform.Find("BulletTrail").GetComponent<TrailRenderer>().Clear();
                return bullet;
            }
            else
            {
                return Instantiate(bulletPrefab, position, Quaternion.identity);
            }
        }
        else if (type == Types.explosion)
        {
            if (explosionPool.Count > 0)
            {
                GameObject explosion = explosionPool[0];
                explosionPool.RemoveAt(0);
                explosion.transform.position = position;
                explosion.SetActive(true);
                return explosion;
            }
            else
            {
                return Instantiate(explosionPrefab, position, Quaternion.identity);
            }
        }
        return null;
    }
    public void InitializeLevel()
    {
        bulletPool.Clear();
        explosionPool.Clear();
    }
   
    #endregion
}