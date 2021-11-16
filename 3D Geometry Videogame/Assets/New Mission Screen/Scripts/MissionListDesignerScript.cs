using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionListDesignerScript : MonoBehaviour
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
        }
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
