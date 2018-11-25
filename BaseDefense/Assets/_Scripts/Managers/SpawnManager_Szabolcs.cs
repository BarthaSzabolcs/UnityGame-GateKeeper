using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_Szabolcs : MonoBehaviour
{
    #region ShowInEditor

    [Header("Spawn:")]
    [SerializeField] GameObject[] enemies;
    [SerializeField] Vector2[] spawnPoints;
    [SerializeField] string spawnKey;

    [Header("AutoSpawn: ")]
    [SerializeField] bool autoSpawn;
    [SerializeField] float spawnDelay;

    #endregion
    #region HideInEditor

    public static SpawnManager_Szabolcs Instance { get; private set; }

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
    private void Start()
    {
        if(autoSpawn)
        {
            StartCoroutine(AutoSpawn());
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnEnemy();
        }
    }
    #endregion
    #region CustomFunctions

    private void SpawnEnemy()
    {
        Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
    }
    private IEnumerator AutoSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnEnemy();
        }
    }

    #endregion
}