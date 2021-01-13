using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    #pragma warning disable IDE0051 // Removes warning for 'unused' methods (like Awake() and Update())

    public GameObject enemyInstanciable;
    public int enemiesToSpawn = 10;
    public int enemiesSpawnLimit = 6;
    public int enemiesSpawnLimitPerSpawnLocation;
    public float enemiesSpawnDelayInMiliseconds = 1000f;
    public bool enemiesReadyToSpawn = false;
    public Transform[] spawnLocations;
    public int spawnLocationsAmount;
    public LayerMask enemyCollisionMask;
    private int enemyId = 1;

    void Start()
    {
        spawnLocationsAmount = spawnLocations.Length;
        enemiesSpawnLimitPerSpawnLocation = Mathf.FloorToInt(enemiesSpawnLimit / spawnLocationsAmount);
        enemiesReadyToSpawn = true;
    }

    void Update()
    {
        if (enemiesReadyToSpawn && enemiesToSpawn > 0)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        int randomNumber = Random.Range(0, spawnLocationsAmount);
        var currentSpawnLocation = spawnLocations[randomNumber];
        enemiesReadyToSpawn = false;

		if (!Physics.CheckCapsule(currentSpawnLocation.position + new Vector3(0, 0.4f, 0), currentSpawnLocation.position - new Vector3(0, 0.4f, 0), 0.4f, enemyCollisionMask)) {
			if (spawnLocations[randomNumber].childCount < enemiesSpawnLimitPerSpawnLocation) {
				GameObject enemy = Instantiate(enemyInstanciable, spawnLocations[randomNumber]);
                enemy.name = ("Enemy_" + EnemyId());
				enemy.SetActive(true);
                enemiesToSpawn--;
                // Enemy will spawn wether in sight of the player or not. Only easy way to check if enemy would be in sight is when it is already spawned, which makes its MeshRenderer.isVisible true. A bodge fix may be done where materials are invisible, but I'd rather leave it as is for now.
		        // Perhaps try this: https://forum.unity.com/threads/making-object-invisible.498089/
            }
		}

        Invoke("PrepareNextEnemySpawn", (enemiesSpawnDelayInMiliseconds / 1000));
    }

    private void PrepareNextEnemySpawn()
    {
        enemiesReadyToSpawn = true;
    }

    private int EnemyId()
    {
        if (enemyId > 10000)
            return enemyId = 1;

        return enemyId++;
    }
}
