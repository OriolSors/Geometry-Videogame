using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTatamiManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 1;

    private BallTatamiController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        playerControllerScript = GameObject.Find("Player").GetComponent<BallTatamiController>();
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        SpawnEnemyWave(waveNumber);
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0 && !playerControllerScript.gameOver)
        {
            waveNumber++;
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            SpawnEnemyWave(waveNumber);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return spawnPos;
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
}
