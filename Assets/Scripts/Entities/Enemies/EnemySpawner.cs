using UnityEngine;
using System.Collections;

/*
 * Written by PurrfectPanda
 */
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; protected set; }

    [SerializeField]
    private Transform[] spawnObjects;
    [SerializeField]
    private Transform[] spawnPoints;
    private GameObject enemyParent;

    private bool enemySlowdown;
    public bool EnemySlowdown
    {
        get { return enemySlowdown; }
        set { enemySlowdown = value; }
    }

    [SerializeField]
    private uint enemySlowdownDuration = 10;
    private float enemySlowdownTimer;

    // first spawn is always one enemy after 2 seconds
    private int currentWaveSize = 1;
    private float timeUntilNextSpawn = 1.0f;

    private void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be two enemy spawners!");
        }
        Instance = this;

        enemySlowdownTimer = enemySlowdownDuration;
        enemyParent = new GameObject("Enemies");
        enemyParent.transform.SetParent(transform);
    }

    private void Update()
    {
        // could use a timer for this
        timeUntilNextSpawn -= Time.deltaTime;
        if (timeUntilNextSpawn > 0.0f)
        {
            return; // not time to spawn anything yet
        }

        int objectIndex = GetRandomSpawnObject();
        int locationIndex = GetRandomSpawnLocation();

        Transform enemy = (Transform)Instantiate(spawnObjects[objectIndex], spawnPoints[locationIndex].position, Quaternion.identity);
        enemy.SetParent(enemyParent.transform);

        if (enemySlowdown)
        {
            enemySlowdownTimer -= Time.deltaTime;
            enemy.GetComponent<EnemyController>().AttackRate = 1.5f;
        }

        if(enemySlowdownTimer < 0)
        {
            enemySlowdown = false;
            enemySlowdownTimer = enemySlowdownDuration;
        }

        --currentWaveSize;


        timeUntilNextSpawn = Random.Range(GetNewMinTime(), GetNewMaxTime());
        currentWaveSize = Random.Range(GetNewMinWave(), GetNewMaxWave());
    }

    // Given an array of weights, will return the index of a weighted random choice
    // E.g, if given {0.8f, 0.2f}; there is an 80% chance to return 0 and 20% to return 1
    private int GetRandomWeightedIndex(float[] weights)
    {
        // sum weights
        float totalweight = 0.0f;
        foreach (float entry in weights)
        {
            totalweight += entry;
        }

        float randomValue = Random.Range(0.0f, totalweight);

        int index = 0;
        float accumulator = 0.0f;
        while (index < weights.Length)
        {
            accumulator += weights[index];
            if (accumulator >= randomValue)
            {
                return index;
            }
            ++index;
        }

        // shouldn't happen?
        return weights.Length - 1;
    }

    // Edit this to determine which spawn location to choose from the supplied ones
    private int GetRandomSpawnLocation()
    {
        float[] locationWeights;
        if (Time.time < 20.0f)
        {
            locationWeights = new float[] { 1.0f, 0.0f };
        }
        else if (Time.time < 60.0f)
        {
            locationWeights = new float[] { 0.8f, 0.2f };
        }
        else
        {
            locationWeights = new float[] { 0.5f, 0.5f };
        }

        return Mathf.Clamp(GetRandomWeightedIndex(locationWeights), 0, spawnPoints.Length - 1);
    }

    // Edit this to determine which object to spawn from the supplied ones
    private int GetRandomSpawnObject()
    {
        float[] objectWeights;
        if (Time.time < 20.0f)
        {
            objectWeights = new float[] { 1.0f };
        }
        else if (Time.time < 60.0f)
        {
            objectWeights = new float[] { 1.0f };
        }
        else
        {
            objectWeights = new float[] { 1.0f };
        }

        return Mathf.Clamp(GetRandomWeightedIndex(objectWeights), 0, spawnObjects.Length - 1);
    }

    // Edit this to determine the min time to wait before a new spawn
    private float GetNewMinTime()
    {
        if (currentWaveSize > 0)
        {
            return 1.0f;
        }
        else
        {
            if (Time.time < 5.0f)
            {
                return 3.0f;
            }
            else if (Time.time < 20.0f)
            {
                return 6.0f;
            }
            else
            {
                return 8.0f;
            }
        }
    }

    // Edit this to determine the max time to wait before a new spawn
    private float GetNewMaxTime()
    {
        if (currentWaveSize > 0)
        {
            return 3.0f;
        }
        else
        {
            if (Time.time < 5.0f)
            {
                return 6.0f;
            }
            else if (Time.time < 20.0f)
            {
                return 10.0f;
            }
            else
            {
                return 12.0f;
            }
        }
    }

    // Edit this to determine the min size of a new wave
    private int GetNewMinWave()
    {
        if (currentWaveSize > 0) // we are in the middle of a wave, don't change
        {
            return currentWaveSize;
        }
        else
        {
            if (Time.time < 5.0f)
            {
                return 1;
            }
            else if (Time.time < 20.0f)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }

    // Edit this to determine the max size of a new wave
    private int GetNewMaxWave()
    {
        if (currentWaveSize > 0) // we are in the middle of a wave, don't change
        {
            return currentWaveSize;
        }
        else
        {
            if (Time.time < 5.0f)
            {
                return 5;
            }
            else if (Time.time < 20.0f)
            {
                return 6;
            }
            else if(Time.time < 30.0f)
            {
                return 8;
            }
            else
            {
                return 12;
            }
        }
    }
}