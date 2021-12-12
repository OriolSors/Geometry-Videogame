using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class SpawnTatamiManager : MonoBehaviour
{
    public Canvas tutorialCanvas;

    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject cubePrefab;

    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 0;
    public Canvas waveCanvas;

    private BallTatamiController playerControllerScript;

    private GameObject cubeToCollect;

    private Dictionary<int, bool> waveCubes;

    private bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        playerControllerScript = GameObject.Find("Player").GetComponent<BallTatamiController>();
        waveCanvas.enabled = false;      
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0 && !playerControllerScript.gameOver && ready)
        {
            waveNumber++;
            waveCanvas.enabled = true;
            StartCoroutine(IndicatorRoundCoroutine());
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            SpawnEnemyWave(waveNumber);
        }
    }

    IEnumerator IndicatorRoundCoroutine()
    {
        waveCanvas.transform.Find("Number Round Text").GetComponent<TextMeshProUGUI>().text = "ROUND " + waveNumber + "!";
        yield return new WaitForSeconds(1.5f);
        waveCanvas.enabled = false;
    }

    public void SetWavesDict(Dictionary<int,bool> waveCubes)
    {
        this.waveCubes = waveCubes;
    }

    public Dictionary<int, bool> GetWaveCubes()
    {
        return waveCubes;
    }

    public void PlayerReady()
    {
        ready = true;
        tutorialCanvas.enabled = false;
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = UnityEngine.Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = UnityEngine.Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return spawnPos;
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        GameObject newCube;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        if (cubeToCollect != null) Destroy(cubeToCollect);

        if (waveCubes.ContainsKey(waveNumber) && !waveCubes[waveNumber])
        {
            newCube = Instantiate(cubePrefab, GenerateSpawnPosition(), cubePrefab.transform.rotation);
            cubeToCollect = newCube;
        }

    }

    public void SetCollectedCube()
    {
        waveCubes[waveNumber] = true;
    }


    
}
