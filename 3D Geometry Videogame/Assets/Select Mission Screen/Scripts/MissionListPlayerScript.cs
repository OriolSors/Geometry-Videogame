using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MissionListPlayerScript : MonoBehaviour
{
    private string username;

    public GameObject missionButton;

    private DatabaseReference reference;

    public RectTransform missionsScroll;

    public Font font;

    void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        LoadUser();
        LoadMissionsWithUser(FillMissionScroll);
    }

    private void FillMissionScroll(List<string> missions)
    {

        foreach (string mission in missions)
        {
            GameObject go = Instantiate(missionButton);
            go.GetComponentInChildren<Text>().text = mission;
            go.transform.SetParent(missionsScroll);

            go.GetComponent<Button>().onClick.AddListener(delegate {LoadMinigames(); }); //TODO: passar parametres de la DB per tal d'actualitzar les figures restants als minijocs

        }
    }

    private void LoadMinigames()
    {
        //TODO: gestionar els nivells de cada minijoc i guardar els punts de partida on apareixeran les figures. Si una ja s'ha recollit, aleshores que no torni a apareixer
        //dins els minijocs associats a aquesta missio
        
        SceneManager.LoadScene("Minigame Selection Screen");
    }

    private void LoadMissionsWithUser(Action<List<string>> callbackFunction)
    {
        List<string> missions = new List<string>();
        var DBTask = reference.Child("Users").Child(username).Child("Missions").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("No missions");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot mission in snapshot.Children)
                {
                    missions.Add(mission.Key.ToString());

                }
            }
            callbackFunction(missions);
        });
    }

    public void ToLogin()
    {
        SceneManager.LoadScene("Auth Screen");
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

    [System.Serializable]
    class SaveDataUser
    {
        public string username;

    }
}
