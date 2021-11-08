using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using Firebase.Extensions;
using System;
using UnityEngine.SceneManagement;

public class NewAccountScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField userName;

    [SerializeField]
    private TMP_Dropdown accountTypeOption;

    private DatabaseReference reference;

    private SceneController sceneController;

    private void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        sceneController = GameObject.Find("Scene Controller").GetComponent<SceneController>();
    }

    public void CreateNewUser()
    {

        string name = userName.text;
        string account = accountTypeOption.options[accountTypeOption.value].text;
        IsNewUser(name, account, AssignUser);

    }

    private void AssignUser(bool available, string name, string account)
    {
        if (available)
        {
            User newUser = new User(name, account);
            string json = JsonUtility.ToJson(newUser);
            reference.Child("Users").Child(name).SetRawJsonValueAsync(json);
            sceneController.LoadCorrectScene(name, SceneControllerCallback);
        }
        else
        {
            userName.placeholder.GetComponent<TextMeshProUGUI>().text = "User exists";
            userName.placeholder.color = Color.red;
            userName.text = "";

        }
    }

    private void SceneControllerCallback(bool exist, string advice)
    {
        
    }

    private void IsNewUser(string userName, string accountTypeOption, Action<bool,string,string> callbackFunction)
    {
        bool isNewUser = true;

        var DBTask = reference.Child("Users").Child(userName).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("Full new user");
                
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child("username").Value.ToString() == userName)
                {
                    isNewUser = false;

                }
            }
            callbackFunction(isNewUser, userName, accountTypeOption);
        });

    }

    public class User
    {
        public string username;
        public string account;

        public User(string username, string account)
        {
            this.username = username;
            this.account = account;
        }
    }

}
