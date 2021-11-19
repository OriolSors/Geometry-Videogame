using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    private DatabaseReference reference;


    private void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
    }

    public void LoadCorrectScene(string name, Action<string> callbackFunction)
    {
        var DBTask = reference.Child("Users").Child(name).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                callbackFunction("User not exist");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                SaveUser(snapshot.Child("username").Value.ToString());
                if (snapshot.Child("account").Value.ToString() == "Player")
                {
                    SceneManager.LoadScene("Player Mission List Screen");
                }
                else
                {
                    DontDestroyOnLoad(transform.gameObject);
                    SceneManager.LoadScene("Designer Mission List Screen");
                }

            }
        });
    }

    private void SaveUser(string username)
    {
        SaveDataUser data = new SaveDataUser();
        data.username = username;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/saveuser.json", json);
    }

    [System.Serializable]
    class SaveDataUser
    {
        public string username;

    }

}
