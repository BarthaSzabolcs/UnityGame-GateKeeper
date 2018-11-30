using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_B_Adam : MonoBehaviour   
{
    // Add Empty Gameobject where to spawn to.
    public GameObject[] SpawnPoints;

    // Mode Selector
    public enum SpawnMode
    {
        Random_waves
    }
    public SpawnMode spawnMode;

    // Which Ai-s to spawn. Currently supports 4 Ai-s.
    public GameObject[] AisToSpawn;

    // This pops up after each wave.
    public GameObject FinishText;

    public GameObject DoneText;

    public int numberOfWaves;
    public int enemiesPerWave;

    public static int enemiesAlive = 0;

    private int numberOfAis;
    private bool readyToStart;
    private bool JButtonWasPressed;
    private bool spawning = false;
    private int currentWave;
    private List<GameObject> Wave = new List<GameObject>();

    // Initialize Random
    System.Random random = new System.Random();
    

    // Start
    void Start()
    {
        DoneText.SetActive(false);
        numberOfAis = AisToSpawn.Length;
        Debug.Log("Number of ais: " + numberOfAis);

        readyToStart = true;
        currentWave = 0;

        if (spawnMode == SpawnMode.Random_waves)
        {
            BuildRandomWaves(AisToSpawn);
        }         
    }

    // Update
    void Update()
    {
        Debug.Log("Enemies alive: " + enemiesAlive);
        if (enemiesAlive == 0 && currentWave != numberOfWaves)
        {
            FinishText.SetActive(true);
        }

        if (readyToStart && Input.GetKeyDown(KeyCode.J) && !spawning && enemiesAlive == 0)
        {
            FinishText.SetActive(false);
            StartCoroutine(CallNextWave());
            UnityEngine.Debug.Log("Next wave!");
        }

        if (currentWave == numberOfWaves && enemiesAlive == 0)
        {
            FinishText.SetActive(false);
            DoneText.SetActive(true);
        }
    }

    // Calling next wave.
    private IEnumerator CallNextWave()
    {
        readyToStart = false;

        // Call fixed waves
        for (int i = 0 + (currentWave* enemiesPerWave); i < currentWave* enemiesPerWave + enemiesPerWave; i++)
        {
            if (i >= Wave.Count)
            {
                FinishText.SetActive(false);
                yield break;
            }

            SpawnEnemy(i);

            if (i == (currentWave* enemiesPerWave + enemiesPerWave) -1)
            {
                spawning = false;
            }else
            {
                spawning = true;
            }
            

            yield return new WaitForSeconds(2.0f);
        }

        currentWave += 1;
        
        readyToStart = true;
    }

    // Instantiate each enemy at a random spawnpoint.
    private void SpawnEnemy(int currentEnemy)
    {
        int randomNumber = random.Next(0, SpawnPoints.Length);
        Vector3 spawnPosition = SpawnPoints[randomNumber].transform.position;
        Instantiate(Wave[currentEnemy], spawnPosition, Wave[currentEnemy].transform.rotation);
        enemiesAlive += 1;
    }

    // Build fixed waves.
    private void BuildRandomWaves(GameObject[] aisToSpawn)
    { 
        for ( int i = 0; i < numberOfWaves; i++)
        {
            for ( int j = 0; j < enemiesPerWave; j++)
            {
                int randomNum = random.Next(0, numberOfAis);
                Wave.Add(AisToSpawn[randomNum]);
            }
        }
    }
}
