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



    public Canvas labelsCanvas;

    private string username;

    private DatabaseManager labelsManagerDB;

    void Start()
    {
        labelsManagerDB = labelsCanvas.GetComponent<DatabaseManager>();
        labelsCanvas.enabled = false;

        cubePositions = new List<Vector3>();
        cubePositions.Add(Vector3.zero);
        preColor = objectsLeft.color;

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
        string path = Application.persistentDataPath + "/saveuser.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataUser data = JsonUtility.FromJson<SaveDataUser>(json);

            this.username = data.username;
        }
    }

    public void SaveConstruction()
    {
        labelsCanvas.enabled = true;
        labelsManagerDB.SetUp(username, cubePositions.Count);
        /*
        Mission mission = new Mission(username, cubePositions.Count);
        string json = JsonUtility.ToJson(mission);
        reference.Child("Users").Child(username).Child("Missions").Child(DateTime.Now.ToString()).SetRawJsonValueAsync(json); //TODO: Change to Push
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        */
    }

    [System.Serializable]
    class SaveDataUser
    {
        public string username;

    }
}
