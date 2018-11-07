 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Manager : MonoBehaviour
{
	#region ShowInEditor
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private GameObject explosionPrefab;
    #endregion
    #region HideInEditor
    public static ObjectPool_Manager Instance { get; private set; }
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
    public void InitializeLevel()
    {
        bulletPool.Clear();
        explosionPool.Clear();
    }
    public void PoolBullet(GameObject bullet)
	{
		bullet.SetActive(false);
		bulletPool.Add(bullet);
	}
	public GameObject SpawnBullet(Vector2 position)
	{
		if (bulletPool.Count > 0)
		{
			GameObject bullet = bulletPool[0];
			bullet.SetActive(true);
			bulletPool.RemoveAt(0);
			bullet.transform.position = position;
			return bullet;
		}
		else
		{
			return Instantiate(bulletPrefab, position, Quaternion.identity);
		}
	}
	public void PoolExplosion(GameObject explosion)
	{
		explosion.SetActive(false);
		explosionPool.Add(explosion);
	}
	public GameObject SpawnExplosion(Vector2 position)
	{
		if(explosionPool.Count > 0)
		{
			GameObject explosion = explosionPool[0];
			explosionPool.RemoveAt(0);
			explosion.SetActive(true);
			explosion.transform.position = position;
			return explosion;
		}
		else
		{
			return Instantiate(explosionPrefab, position, Quaternion.identity);
		}
	}
    #endregion
}