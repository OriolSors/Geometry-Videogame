using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Database;
using System;
using System.IO;

public class ConstructionController : MonoBehaviour
{

    public List<Vector3> cubePositions;
    public TextMeshProUGUI objectsLeft;
    private Color preColor;

    private DatabaseReference reference;


    private string username;

    void Start()
    {
        cubePositions = new List<Vector3>();
        cubePositions.Add(Vector3.zero);
        preColor = objectsLeft.color;

        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

        LoadUser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetConstruction()
    {
        SceneManager.LoadScene("3D Editor");
    }

    public void AddNewObject(Vector3 position)
    {

        cubePositions.Add(position);
        objectsLeft.text = (int.Parse(objectsLeft.text) - 1).ToString();
        SetColor();
    }

    public void RemoveObject(Vector3 position)
    {
        cubePositions.Remove(position);
        objectsLeft.text = (int.Parse(objectsLeft.text) + 1).ToString();
        SetColor();
    }

    public bool ObjectsAvailables()
    {
        return int.Parse(objectsLeft.text) > 0;
    }

    private void SetColor()
    {
        if (int.Parse(objectsLeft.text) == 0)
        {
            objectsLeft.color = Color.red;
        }
        else
        {
            objectsLeft.color = preColor;
        }
    }

    private void LoadUser()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            this.username = data.username;
        }
    }

    public void SaveConstruction()
    {
        Mission mission = new Mission(username, cubePositions.Count);
        string json = JsonUtility.ToJson(mission);
        reference.Child("Users").Child(username).Child("Missions").Child(DateTime.Now.ToString()).SetRawJsonValueAsync(json); //TODO: Change to Push
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string username;

    }

    public class Mission
    {
        public string name;
        public int cubes;

        public Mission(string name, int cubes)
        {
            this.name = name;
            this.cubes = cubes;
        }
    }
}
