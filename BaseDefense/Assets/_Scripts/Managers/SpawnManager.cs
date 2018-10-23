using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region ShowInEditor
    [SerializeField] GameObject B_ADAM_ENEMY;
    [SerializeField] GameObject[] enemies;
    [SerializeField] Vector2[] spawnPoints;
    #endregion
    #region HideInEditor
    public SpawnManager Instance { get; private set; }
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
        // B. Ádám AI-ja itt spawnol teszt jelleggel.
        Instantiate(B_ADAM_ENEMY, new Vector3(40, -2, 0), Quaternion.identity);

    }

    private void Update()
    {
        if (Input.GetKeyDown("mouse 2"))
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
