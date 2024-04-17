using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject objective;
    public Transform[] path;
    public float spawnTime = 2; // seconds between each spawn
    public int numEnemies = 5;
    public float enemySpeed = 1;
    public bool spawnOnStart = true;
    public int wave = 0;

    public GameObject enemyPrefab;

    float timeSinceLastSpawn;
    bool spawning;

    void Start()
    {
        spawning = false;
        if (objective == null) {
            objective = GameObject.FindGameObjectWithTag("Objective");
        }
        LevelManager.enemiesRemaining[wave] += numEnemies;
    }

    void Update()
    {
        if (LevelManager.currentPhase != LevelManager.GamePhase.EnemyWave || LevelManager.wave != wave) return;
        if (!spawning)
        {
            spawning = true;
            timeSinceLastSpawn = 0;
            if (spawnOnStart) {
                SpawnEnemy();
            }
        }
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnTime)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (numEnemies > 0)
        {
            var newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            newEnemy.transform.SetParent(transform);
            newEnemy.GetComponent<FollowPath>().SetPath(path, objective, enemySpeed);
            timeSinceLastSpawn = 0;
            numEnemies--;
        }
    }
}
