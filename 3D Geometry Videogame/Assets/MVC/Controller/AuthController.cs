using System;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public sealed class AuthController
{

    private DatabaseReference reference;
    private Firebase.Auth.FirebaseAuth auth;

    private MissionListController missionListController;

    private User currentUser = null;

    private AuthController()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        missionListController = new MissionListController();
    }

    private static AuthController instance = null;
    public static AuthController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AuthController();
            }
            return instance;
        }
    }

    public void RegisterNewUser(string username, string email, string password, string accountType, Action<string> ConfirmUserLogged, Action<string> GetNegativeResultOfUserCreation)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                GetNegativeResultOfUserCreation(task.Exception.ToString());
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                GetNegativeResultOfUserCreation("Invalid register. Try another email, password or both.");
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            CreateNewUser(username, email, accountType);
            SetCurrentUser(email, ConfirmUserLogged);
        });
    }

    public void LoginUser(string email, string password, Action<string> ConfirmUserLogged, Action<string> GetNegativeResultOfUserLogged)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                GetNegativeResultOfUserLogged(task.Exception.ToString());
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                GetNegativeResultOfUserLogged("User not exists.");
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            SetCurrentUser(email, ConfirmUserLogged);
        });
    }

    private void CreateNewUser(string username, string email, string accountType)
    {
        User newUser = null;
        switch (accountType)
        {
            case "Designer":
                newUser = new Designer(username, email);
                break;
            case "Player":
                newUser = new Player(username, email);
                break;
        }

        if (newUser != null) newUser.WriteNewUserToDB();
    }

    private void SetCurrentUser(string email, Action<string> ConfirmUserLogged)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            var DBTask = reference.Child("Users").Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.Result.Value == null)
                {
                    Debug.Log("User not exist");

                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    string accountType = snapshot.Child("account").Value.ToString();

                    switch (accountType)
                    {
                        case "Designer":
                            currentUser = new Designer(snapshot.Child("username").Value.ToString(), email);
                            break;
                        case "Player":
                            currentUser = new Player(snapshot.Child("username").Value.ToString(), email);
                            break;
                    }

                    if (currentUser != null)
                    {
                        missionListController.RetrieveAllMissionUser(currentUser, snapshot.Child("listOfMissions").Children);
                        currentUser.WriteUserToLocalJSON();
                        ConfirmUserLogged(accountType);
                    }

                }

            });
        }
        
    }

    public User GetCurrentUser()
    {
        return currentUser;
    }

    public void SignOut()
    {
        auth.SignOut();
    }



}
