using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class SpawnTatamiManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject cubePrefab;

    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 0;

    private BallTatamiController playerControllerScript;

    private GameObject cubeToCollect;

    private bool ready = false;
    private Dictionary<int, bool> waveCubes = new Dictionary<int, bool>();
    private DatabaseReference reference;

    private string username, mission;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        playerControllerScript = GameObject.Find("Player").GetComponent<BallTatamiController>();

        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

        LoadUser();
        LoadCurrentMission();
        LoadWaveParameters(SetWaveCubes);       
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0 && !playerControllerScript.gameOver && ready)
        {
            waveNumber++;
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            SpawnEnemyWave(waveNumber);
        }
    }

    private void SetWaveCubes(Dictionary<int, bool> waveCubes)
    {
        this.waveCubes = waveCubes;
        ready = true;
    }

    private void LoadWaveParameters(Action<Dictionary<int, bool>> callbackFunction)
    {
        Dictionary<int, bool> waveCubes = new Dictionary<int, bool>();
        var DBTask = reference.Child("Users").Child(username).Child("Missions").Child(mission).Child("waveCubeSpawn").Child("tatami").GetValueAsync().ContinueWithOnMainThread(task =>
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
