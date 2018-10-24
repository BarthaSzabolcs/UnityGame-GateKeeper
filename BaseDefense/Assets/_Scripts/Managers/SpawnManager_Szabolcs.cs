using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_Szabolcs : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] GameObject[] enemies;
    [SerializeField] Vector2[] spawnPoints;
    [SerializeField] string spawnKey;
    #endregion
    #region HideInEditor
    public SpawnManager_Szabolcs Instance { get; private set; }
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
    private void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnEnemy();
        }
    }
    #endregion


    public void SpawnEnemy()
    {
        Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
    }
}
