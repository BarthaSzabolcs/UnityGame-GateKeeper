using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_Szabolcs : MonoBehaviour
{
    #region Events

    public delegate void WaveStarted();
    public event WaveStarted OnWaveStarted;

    public delegate void WaveEnded();
    public event  WaveEnded OnWaveEnded;

    #endregion
    #region ShowInEditor

    [Header("SpawnWaves:")]
    [SerializeField] SpawnWaveData[] spawnWaves;
    [SerializeField] int[] waveStrengths;

    [Header("Spawn:")]
    [SerializeField] Vector2[] GroundSpawns;
    [SerializeField] Vector2[] AirSpawns;

    [Header("Rarity weights:")]
    [SerializeField] float commonChance;
    [SerializeField] float rareChance;
    [SerializeField] float epicChance;
    [SerializeField] float legendaryChance;

    #endregion
    #region HideInEditor

    public static SpawnManager_Szabolcs Instance { get; private set; }

    private int enemiesAlive;

    private int waveStrength_Index;
    private int WaveStrength_Index
    {
        get
        {
            return waveStrength_Index;
        }
        set
        {
            if (value >= waveStrengths.Length)
            {
                waveStrength_Index = waveStrengths.Length - 1;
            }
            else if (value < 0)
            {
                waveStrength_Index = 0;
            }
            else
            {
                waveStrength_Index = value;
            }
        }
    }
    private int CurrentWaveStrength
    {
        get
        {
            return waveStrengths[WaveStrength_Index];
        }
    }

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

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
        OnWaveStarted?.Invoke();
    }

    private float RarityInFloat(EnemyData.Rarity rarity)
    {
        if (rarity == EnemyData.Rarity.Common)
        {
            return commonChance;
        }
        else if (rarity == EnemyData.Rarity.Rare)
        {
            return rareChance;
        }
        else if (rarity == EnemyData.Rarity.Epic)
        {
            return epicChance;
        }
        else if (rarity == EnemyData.Rarity.Legendary)
        {
            return legendaryChance;
        }
        else
        {
            return 0f;
        }
    }
    private List<EnemyData> CreateWave(int targetedWaveStrength)
    {
        List<EnemyData> enemies = new List<EnemyData>();

        int waveIndex = Random.Range(0, spawnWaves.Length);

        float weightSum = 0;
        foreach (EnemyData enemy in spawnWaves[waveIndex].enemies)
        {
            weightSum += RarityInFloat(enemy.rarity);
        }

        int waveStrength = 0;
        while (waveStrength < targetedWaveStrength)
        { 
            float weightedRandom = Random.Range(0f, weightSum);
            int weightedEnemyIndex = 0;
            foreach (EnemyData enemy in spawnWaves[waveIndex].enemies)
            {
                int index = 0;

                if (weightedRandom - RarityInFloat(enemy.rarity) < 0)
                {
                    weightedEnemyIndex = index;
                    break;
                }
                else
                {
                    weightedRandom -= RarityInFloat(enemy.rarity);
                    index++;
                }
            }

            enemies.Add(spawnWaves[waveIndex].enemies[weightedEnemyIndex]);

            waveStrength += spawnWaves[waveIndex].enemies[weightedEnemyIndex].strengh;
        }

        return enemies;
    }

    private IEnumerator SpawnWave()
    {
        foreach (EnemyData enemy in CreateWave(CurrentWaveStrength))
        {
            SpawnEnemy(enemy);
            yield return new WaitForSeconds(enemy.spawndelay);
        }

    }
    private void SpawnEnemy(EnemyData enemyData)
    {
        Vector2 spawnPosition = Vector2.zero;

        if(enemyData.type == EnemyData.Type.Ground)
        {
            spawnPosition = GroundSpawns[Random.Range(0, GroundSpawns.Length)];
        }
        else if (enemyData.type == EnemyData.Type.Air)
        {
            spawnPosition = AirSpawns[Random.Range(0, GroundSpawns.Length)];
        }

        GameObject spawnedEnemy = Instantiate(enemyData.enemy, spawnPosition, Quaternion.identity);
        spawnedEnemy.GetComponent<Health>().OnDeath += HandleEnemyDeath;

        enemiesAlive++;
    }

    private void HandleEnemyDeath(GameObject sender)
    {
        enemiesAlive--;

        if (enemiesAlive == 0)
        {
            OnWaveEnded?.Invoke();
            WaveStrength_Index++;
        }
    }

    #endregion
}