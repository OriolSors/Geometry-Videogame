using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionListDesignerScript : MonoBehaviour
{

    private string username;

    public RectTransform missionsScroll;
    public GameObject missionView;

    public Canvas userStatistics;
    public RectTransform userStatisticsScroll;
    public GameObject userView;

    protected Firebase.Auth.FirebaseAuth auth;
    private DatabaseReference reference;
    

    public Font font;

    void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        userStatistics.enabled = false;

        LoadUser();
        LoadMissionsWithUser(FillMissionScroll);
    }

    private void FillMissionScroll(List<string> missions)
    {

        foreach (string mission in missions)
        {
            GameObject go = Instantiate(missionView);
            go.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = mission;
            go.transform.SetParent(missionsScroll);

            go.GetComponentInChildren<Button>().onClick.AddListener(delegate { LoadUserStatistics(mission, FillUserStatistics); });
        }
    }

    private void FillUserStatistics(Dictionary <string, string> players)
    {
        userStatistics.enabled = true;

        foreach (Transform child in userStatisticsScroll.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string user in players.Keys)
        {
            GameObject go = Instantiate(userView);
            go.transform.Find("Username Text").GetComponent<TextMeshProUGUI>().text = user;
            go.transform.Find("Status Text").GetComponent<TextMeshProUGUI>().text = players[user];
            go.transform.SetParent(userStatisticsScroll);

        }
    }

    private void LoadUserStatistics(string mission, Action<Dictionary<string, string>> callbackFunction)
    {

        Dictionary<string, string> players = new Dictionary<string, string>();
        var DBTask = reference.Child("Missions").Child(mission).GetValueAsync().ContinueWithOnMainThread(task =>
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
                int cubes = int.Parse(snapshot.Child("cubes").Value.ToString());
                foreach (DataSnapshot user in snapshot.Child("playersDict").Children)
                {
                    int inventory = int.Parse(user.Child("inventory").Value.ToString());
                    if (inventory / cubes >= 1)
                    {
                        players[user.Key.ToString()] = "100%";
                    }
                    else
                    {
                        players[user.Key.ToString()] = (100 * inventory/cubes).ToString() + "%";
                    }
                }
            }

            callbackFunction(players);
            
        });
    }

    public void ReturnMissionList()
    {
        userStatistics.enabled = false;
    }

    private void LoadMissionsWithUser(Action<List<string>> callbackFunction)
    {
        List<string> missions = new List<string>();
        var DBTask = reference.Child("Missions").GetValueAsync().ContinueWithOnMainThread(task =>
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
                foreach (DataSnapshot s in snapshot.Children)
                {
                    if (s.Child("designer").Value.ToString() == username) missions.Add(s.Key.ToString());
                }
            }
            callbackFunction(missions);
        });
    }


    public void ToEditor3D()
    {
        SceneManager.LoadScene("3D Editor");
    }

    public void ToLogin()
    {
        auth.SignOut();
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
