using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private int enemySpawnLimit = 10;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    [SerializeField]
    private float minSpawnTime = 2f, maxSpawnTime = 5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Invoke("SpawnEnemy", Random.Range(minSpawnTime, maxSpawnTime));
    }

    void SpawnEnemy()
    {
        Invoke("SpawnEnemy", Random.Range(minSpawnTime, maxSpawnTime));

        if (spawnedEnemies.Count < enemySpawnLimit)
        {
            GameObject randomEnemy = enemyPrefabs[
                Random.Range(0, enemyPrefabs.Length)
            ];

            GameObject newEnemy = Instantiate(
                randomEnemy,
                spawnPosition[Random.Range(0, spawnPosition.Length)].position,
                Quaternion.identity
            );

            spawnedEnemies.Add(newEnemy);
        }
    }


    public void EnemyDied(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
    }

}
