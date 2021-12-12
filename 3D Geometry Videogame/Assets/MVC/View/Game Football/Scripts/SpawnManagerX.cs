using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManagerX : MonoBehaviour
{
    public Canvas tutorialCanvas;

    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject cubePrefab;

    private float spawnRangeX = 10;
    private float spawnZMin = 15; // set min spawn Z
    private float spawnZMax = 25; // set max spawn Z

    public int enemyCount;
    public int waveCount = 0;
    public Canvas waveCanvas;

    public float increaseSpeed = 0;

    public bool gameOver = false;
    public Canvas scoreboard;
    public Canvas gameOverCanvas;

    private int playerScore = 0;
    private int enemyScore = 0;

    public GameObject player;
    private PlayerControllerX playerScript;

    private GameObject cubeToCollect;

    private bool ready = false;
    private Dictionary<int, bool> waveCubes;

    private void Start()
    {
        playerScript = player.GetComponent<PlayerControllerX>();
        waveCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameOver = (enemyScore - playerScore) == 5;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0 && !gameOver && ready)
        {
            waveCount++;
            waveCanvas.enabled = true;
            StartCoroutine(IndicatorRoundCoroutine());
            SpawnEnemyWave(waveCount);
        }
        else if (gameOver)
        {
            gameOverCanvas.enabled = true;
            StartCoroutine(IndicatorGameOverCoroutine());
        }

    }

    IEnumerator IndicatorGameOverCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        gameOverCanvas.enabled = false;
        playerScript.ExitGame();
    }

    IEnumerator IndicatorRoundCoroutine()
    {
        waveCanvas.transform.Find("Number Round Text").GetComponent<TextMeshProUGUI>().text = "ROUND " + waveCount + "!";
        yield return new WaitForSeconds(1.5f);
        waveCanvas.enabled = false;
    }

    public void SetWavesDict(Dictionary<int, bool> waveCubes)
    {
        this.waveCubes = waveCubes;
    }

    public void PlayerReady()
    {
        ready = true;
        tutorialCanvas.enabled = false;
    }

    public void NewGoalPlayer()
    {
        playerScore++;
        scoreboard.transform.Find("Player Goals Number").GetComponent<TextMeshProUGUI>().text = playerScore.ToString();
    }

    public void NewGoalEnemy()
    {
        enemyScore++;
        scoreboard.transform.Find("Enemy Goals Number").GetComponent<TextMeshProUGUI>().text = enemyScore.ToString();
    }

    // Generate random spawn position for powerups and enemy balls
    Vector3 GenerateSpawnPosition ()
    {
        float xPos = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = UnityEngine.Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }


    void SpawnEnemyWave(int enemiesToSpawn)
    {
        GameObject newCube;

        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15); // make powerups spawn at player end

        // If no powerups remain, spawn a powerup
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0) // check that there are zero powerups
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        // Spawn number of enemy balls based on wave number
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        if (cubeToCollect != null) Destroy(cubeToCollect);

        if (waveCubes.ContainsKey(waveCount) && !waveCubes[waveCount])
        {
            newCube = Instantiate(cubePrefab, GenerateSpawnPosition(), cubePrefab.transform.rotation);
            cubeToCollect = newCube;
        }


        increaseSpeed += 10;
        ResetPlayerPosition(); // put player back at start

    }

    // Move player back to position in front of own goal
    void ResetPlayerPosition ()
    {
        player.transform.position = new Vector3(0, 1, -7);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }

    public void SetCollectedCube()
    {
        waveCubes[waveCount] = true;
    }

    public Dictionary<int, bool> GetWaveCubes()
    {
        return waveCubes;
    }


}
