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

    public GameObject missionView;

    private DatabaseReference reference;

    public RectTransform missionsScroll;

    public Font font;

    void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        LoadUser();
        LoadMissionsWithUser(FillMissionScroll);
    }

    private void FillMissionScroll(Dictionary<string, List<int>> missions)
    {

        foreach (string mission in missions.Keys)
        {
            GameObject go = Instantiate(missionView);
            go.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = mission;
            go.transform.Find("Progression Text").GetComponent<Text>().text = (100 * missions[mission][0] / missions[mission][1]).ToString() + "%";
            go.transform.SetParent(missionsScroll);

            go.GetComponentInChildren<Button>().onClick.AddListener(delegate {LoadMinigames(); }); //TODO: passar parametres de la DB per tal d'actualitzar les figures restants als minijocs

        }
    }

    private void LoadMinigames()
    {
        //TODO: gestionar els nivells de cada minijoc i guardar els punts de partida on apareixeran les figures. Si una ja s'ha recollit, aleshores que no torni a apareixer
        //dins els minijocs associats a aquesta missio
        
        SceneManager.LoadScene("Minigame Selection Screen");
    }

    private void LoadMissionsWithUser(Action<Dictionary<string, List<int>>> callbackFunction)
    {
        Dictionary<string, List<int>> missions = new Dictionary<string, List<int>>();

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
                    List<int> progression = new List<int>();
                    progression.Add(int.Parse(mission.Child("inventory").Value.ToString()));
                    progression.Add(int.Parse(mission.Child("cubes").Value.ToString()));
                    missions[mission.Key.ToString()] = progression;
                    
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

            username = data.username;
        }
    }

    [System.Serializable]
    class SaveDataUser
    {
        public string username;

    }
}
