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
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject cubePrefab;

    private float spawnRangeX = 10;
    private float spawnZMin = 15; // set min spawn Z
    private float spawnZMax = 25; // set max spawn Z

    public int enemyCount;
    public int waveCount = 0;
    public float increaseSpeed = 0;

    public bool gameOver = false;
    public Canvas scoreboard;

    private int playerScore = 0;
    private int enemyScore = 0;

    public GameObject player;
    private PlayerControllerX playerScript;

    private GameObject cubeToCollect;

    private bool ready = false;
    private Dictionary<int, bool> waveCubes = new Dictionary<int, bool>();
    private DatabaseReference reference;

    private string username, mission;

    private void Start()
    {
        playerScript = player.GetComponent<PlayerControllerX>();

        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

        LoadUser();
        LoadCurrentMission();
        LoadWaveParameters(SetWaveCubes);
    }

    // Update is called once per frame
    void Update()
    {
        gameOver = (enemyScore - playerScore) == 5;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0 && !gameOver && ready)
        {
            waveCount++;
            SpawnEnemyWave(waveCount);
        }
        else if (gameOver)
        {
            playerScript.ExitGame();
        }

    }

    private void SetWaveCubes(Dictionary<int, bool> waveCubes)
    {
        this.waveCubes = waveCubes;

        foreach (int waveCount in waveCubes.Keys)
        {
            if (!waveCubes[waveCount])
            {
                this.waveCount = waveCount-1;
                break;
            }
        }

        if (waveCount == -1) waveCount = 0;
        ready = true;
    }

    private void LoadWaveParameters(Action<Dictionary<int, bool>> callbackFunction)
    {
        Dictionary<int, bool> waveCubes = new Dictionary<int, bool>();
        var DBTask = reference.Child("Users").Child(username).Child("Missions").Child(mission).Child("waveCubeSpawn").Child("football").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("No cubes");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot cubeWaveCollected in snapshot.Children)
                {
                    waveCubes[int.Parse(cubeWaveCollected.Key)] = (bool)cubeWaveCollected.Value;
                }
            }

            callbackFunction(waveCubes);
        });
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

    private void LoadUser()
    {
        string path = Application.persistentDataPath + "/saveuser.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataUser data = JsonUtility.FromJson<SaveDataUser>(json);

            username = data.username;
        }
    }

    private void LoadCurrentMission()
    {
        string path = Application.persistentDataPath + "/savecurrentmission.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataCurrentMission data = JsonUtility.FromJson<SaveDataCurrentMission>(json);

            mission = data.mission;
        }
    }

    [System.Serializable]
    class SaveDataUser
    {
        public string username;

    }

    [System.Serializable]
    class SaveDataCurrentMission
    {
        public string mission;
        public int inventory;

    }

}
