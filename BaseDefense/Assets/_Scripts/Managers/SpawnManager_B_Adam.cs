using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_B_Adam : MonoBehaviour   
{
    // Add Empty Gameobject where to spawn to.
    public GameObject[] SpawnPoints;

    // Which Ai-s to spawn. Currently supports 4 Ai-s.
    public GameObject[] AisToSpawn;

    // This pops up after each wave.
    public GameObject FinishText;

    public static int enemiesAlive = 0;

    private int numberOfAis;
    private bool readyToStart;
    private bool JButtonWasPressed;
    private int currentWave;
    private int enemyPerWave = 10;
    private List<GameObject> Wave = new List<GameObject>();

    // Initialize Random
    System.Random random = new System.Random();
    

    // Start
    void Start()
    {
        numberOfAis = AisToSpawn.Length;
        readyToStart = true;
        JButtonWasPressed = false;
        currentWave = 0;
        BuildFixWaves(AisToSpawn);   
    }

    // Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            JButtonWasPressed = true;
            FinishText.SetActive(false);
        }

        if (readyToStart && JButtonWasPressed)
        {
            StartCoroutine(CallNextWave());
            JButtonWasPressed = false;
            UnityEngine.Debug.Log("Next wave!");
        }

        if (enemiesAlive == 0 && !readyToStart)
        {
            readyToStart = true;
        }
        
    }

    // Calling next wave.
    private IEnumerator CallNextWave()
    {
        readyToStart = false;

        // Call fixed waves
        for (int i = 0 + (currentWave*10); i < currentWave*10 + 10; i++)
        {
            if (i >= Wave.Count)
            {
                FinishText.SetActive(false);
                yield break;
            }

            SpawnEnemy(i);

            yield return new WaitForSeconds(2.0f);
        }
        currentWave += 1;
        FinishText.SetActive(true);
    }

    // Instantiate each enemy at a random spawnpoint.
    private void SpawnEnemy(int currentEnemy)
    {
        int randomNumber = random.Next(0, SpawnPoints.Length);
        Vector3 spawnPosition = SpawnPoints[randomNumber].transform.position;
        Instantiate(Wave[currentEnemy], spawnPosition, Wave[currentEnemy].transform.rotation);
    }

    // Build fixed waves.
    private void BuildFixWaves(GameObject[] aisToSpawn)
    {
        // 5 waves total currently with fix 10 ai's per wave.;
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[1]);

        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[2]);

        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[2]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[2]);

        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[2]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[0]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[2]);
        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[2]);
        Wave.Add(AisToSpawn[1]);

        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[2]);
        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[1]);
        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[3]);
        Wave.Add(AisToSpawn[2]);
        Wave.Add(AisToSpawn[3]);
    }
}