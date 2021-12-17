using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Database;
using Firebase.Extensions;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public Material goldMaterial;
    public float dynamicEdge { get; private set; }
    private bool isStreakAchieved = false;

    public Canvas tutorialCanvas;
    private bool ready = false;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -1, 0);
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnPrefabFigure", 1f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPlayerReady()
    {
        return ready;
    }

    public void PlayerReady()
    {
        ready = true;
        tutorialCanvas.enabled = false;
        playerController.HideChallengeCanvas();
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-5, 5);
        float spawnPosZ = Random.Range(-4, -2);
        Vector3 spawnPos = new Vector3(spawnPosX, 12, spawnPosZ);
        return spawnPos;
    }

    private void SpawnPrefabFigure()
    {
        if (ready)
        {
            if (isStreakAchieved)
            {
                isStreakAchieved = false;

                GameObject goldInst;

                foreach (GameObject prefab in prefabs)
                {
                    if (prefab.CompareTag("Cube"))
                    {
                        goldInst = Instantiate(prefab, GenerateSpawnPosition(), prefab.transform.rotation);
                        goldInst.GetComponent<Renderer>().material = goldMaterial;
                        break;
                    }
                }
            }

            else
            {
                GameObject prefabInst = prefabs[Random.Range(0, prefabs.Length)];
                Instantiate(prefabInst, GenerateSpawnPosition(), prefabInst.transform.rotation);
            }
        }
            
    }

    public void StreakAchieved()
    {
        isStreakAchieved = true;
    }

}
