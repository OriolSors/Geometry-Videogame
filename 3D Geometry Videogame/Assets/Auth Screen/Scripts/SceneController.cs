using System;
using System.Collections;
using System.Collections.Generic;
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

    public void LoadCorrectScene(string name, Action<bool,string> callbackFunction)
    {
        var DBTask = reference.Child("Users").Child(name).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                callbackFunction(false,"User not exist");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child("account").Value.ToString() == "Player")
                {
                    SceneManager.LoadScene("Game Selection");
                }
                else
                {
                    SceneManager.LoadScene("3D Editor");
                }
            }
        });
    }


}
