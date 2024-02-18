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

    public GameObject enemyPrefab;

    float timeSinceLastSpawn;

    void Start()
    {
        if (objective == null) {
            objective = GameObject.FindGameObjectWithTag("Objective");
        }
        if (spawnOnStart) {
            SpawnEnemy();
        }
        timeSinceLastSpawn = 0;
    }

    void Update()
    {
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
            timeSinceLastSpawn -= spawnTime;
            numEnemies--;
        }
    }
}
