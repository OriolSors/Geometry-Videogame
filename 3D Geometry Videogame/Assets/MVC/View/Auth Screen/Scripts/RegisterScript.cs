using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using Firebase.Extensions;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class RegisterScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField username;

    [SerializeField]
    private TMP_InputField email;

    [SerializeField]
    private TMP_InputField password;

    [SerializeField]
    private TMP_Dropdown accountTypeOption;

    private DatabaseReference reference;

    private Firebase.Auth.FirebaseAuth auth;

    private SceneController sceneController;

    private void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        sceneController = GameObject.Find("Scene Controller").GetComponent<SceneController>();
    }

    public void CreateNewUser()
    {
        string name = username.text;
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
            sceneController.LoadCorrectScene(name, null);
        }
        else
        {
            username.placeholder.GetComponent<TextMeshProUGUI>().text = "User exists";
            username.placeholder.color = Color.red;
            username.text = "";

        }
    }

    private void IsNewUser(string username, string accountTypeOption, Action<bool,string,string> callbackFunction)
    {
        bool isNewUser = true;

        var DBTask = reference.Child("Users").Child(username).GetValueAsync().ContinueWithOnMainThread(task => {
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
                if (snapshot.Child("username").Value.ToString() == username)
                {
                    isNewUser = false;

                }
            }
            callbackFunction(isNewUser, username, accountTypeOption);
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
