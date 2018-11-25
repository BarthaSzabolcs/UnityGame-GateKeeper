 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region ShowInEditor

    [Header("Bullet:")]
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletPoolBase;

    [Header("Explosion")]
    [SerializeField] private Transform explosionTransform;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int explosionPoolbase;

    [Header("FlyingText:")]
    [SerializeField] private Transform flyingTextTransform;
    [SerializeField] private GameObject flyingTextPrefab;
    [SerializeField] private int flyingTextPoolBase;

    #endregion
    #region HideInEditor
    
    public static ObjectPool Instance { get; private set; }

    public enum Types{ bullet, explosion, flyingText};

    private Camera mainCamera;

    private List<GameObject> bulletPool = new List<GameObject>();
	private List<GameObject> explosionPool = new List<GameObject>();
    private List<GameObject> flyingTextPool = new List<GameObject>();

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
        else if (type == Types.flyingText)
        {
            flyingTextPool.Add(obj);
        }
    }
    public GameObject Spawn(Types type, Vector2 worldPosition)
    {
        if(type == Types.bullet)
        { 
            if (bulletPool.Count > 0)
            {
                GameObject bullet = bulletPool[0];
                bulletPool.RemoveAt(0);

                bullet.transform.position = worldPosition;
                bullet.SetActive(true);
                bullet.transform.Find("BulletTrail").GetComponent<TrailRenderer>().Clear();

                return bullet;
            }
            else
            {
                return Instantiate(bulletPrefab, worldPosition, Quaternion.identity, bulletTransform);
            }
        }
        else if (type == Types.explosion)
        {
            if (explosionPool.Count > 0)
            {
                GameObject explosion = explosionPool[0];
                explosionPool.RemoveAt(0);

                explosion.transform.position = worldPosition;
                explosion.SetActive(true);

                return explosion;
            }
            else
            {
                return Instantiate(explosionPrefab, worldPosition, Quaternion.identity, explosionTransform);
            }
        }
        else if (type == Types.flyingText)
        {
            //Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
            
            if (flyingTextPool.Count > 0)
            {
                GameObject flyingText = flyingTextPool[0];
                flyingTextPool.RemoveAt(0);

                flyingText.transform.position = worldPosition;
                flyingText.SetActive(true);

                return flyingText;
            }
            else
            {
                return Instantiate(flyingTextPrefab, worldPosition, Quaternion.identity, flyingTextTransform);
            }
        }
        return null;
    }

    public void InitializeLevel()
    {
        // Get main camera
        mainCamera = Camera.main;

        // Initialize BulletPool
        for (int i = 0; i < bulletPoolBase; i++)
        {
            Pool( Types.bullet, Instantiate(bulletPrefab, Vector2.zero, Quaternion.identity, bulletTransform));
        }

        // Initialize ExplosionPool
        for (int i = 0; i < explosionPoolbase; i++)
        {
            Pool(Types.explosion, Instantiate(explosionPrefab, Vector2.zero, Quaternion.identity, explosionTransform));
        }

        // Initialize FlyingTextPool
        for (int i = 0; i < flyingTextPoolBase; i++)
        {
            Pool(Types.flyingText, Instantiate(flyingTextPrefab, Vector2.zero, Quaternion.identity, flyingTextTransform));
        }
    }
   
    #endregion
}