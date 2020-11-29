using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject enemyInstanciable;
    public int enemiesToSpawn = 10;
    public int enemiesSpawnLimit = 6;
    public int enemiesSpawnLimitPerSpawnLocation;
    public float enemiesSpawnDelayInMiliseconds = 1000f;
    public bool enemiesReadyToSpawn = false;
    public Transform[] spawnLocations;
    public int spawnLocationsAmount;

    // Start is called before the first frame update
    void Start()
    {
        spawnLocationsAmount = spawnLocations.Length;
        enemiesSpawnLimitPerSpawnLocation = Mathf.FloorToInt(enemiesSpawnLimit / spawnLocationsAmount);
        enemiesReadyToSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesReadyToSpawn && enemiesToSpawn > 0)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        int randomNumber = Random.Range(0, spawnLocationsAmount);
        enemiesReadyToSpawn = false;

        if (spawnLocations[randomNumber].childCount < enemiesSpawnLimitPerSpawnLocation) {
            GameObject enemy = Instantiate(enemyInstanciable, spawnLocations[randomNumber]);
            enemy.SetActive(true);

            enemiesToSpawn--;
        }

        Invoke("PrepareNextEnemySpawn", (enemiesSpawnDelayInMiliseconds / 1000));
    }

    void PrepareNextEnemySpawn()
    {
        enemiesReadyToSpawn = true;
    }
}
