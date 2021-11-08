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
    private int counterCube = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadData());
        InvokeRepeating("SpawnPrefabFigure", 1f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        //GameObject prefabInst = prefabs[1];
        GameObject prefabInst = prefabs[Random.Range(0, prefabs.Length)];
        if (prefabInst.CompareTag("Cube")) {
            counterCube++;
            if (counterCube == 4)
            {
                counterCube = 0;
                GameObject goldInst = Instantiate(prefabInst, GenerateSpawnPosition(), prefabInst.transform.rotation);
                goldInst.GetComponent<Renderer>().material = goldMaterial;
                goldInst.transform.localScale = new Vector3(dynamicEdge, dynamicEdge, dynamicEdge);

            }
            else
            {
                Instantiate(prefabInst, GenerateSpawnPosition(), prefabInst.transform.rotation);
            }
            
        }
        else
        {
            Instantiate(prefabInst, GenerateSpawnPosition(), prefabInst.transform.rotation);
        }
            
        
        
    }

    private IEnumerator LoadData()
    {
        DatabaseReference reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

        var DBTask = reference.Child("Figures").Child("Cube").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.Log("fail");
        }
        else if (DBTask.Result.Value == null)
        {
            dynamicEdge = 4f;
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            dynamicEdge = float.Parse(snapshot.Child("edge").Value.ToString());
        }


    }


}
