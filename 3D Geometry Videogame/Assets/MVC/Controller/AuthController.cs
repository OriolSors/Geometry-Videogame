using System;
using System.IO;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public sealed class AuthController
{

    private DatabaseReference reference;
    private Firebase.Auth.FirebaseAuth auth;

    private User currentUser = null;

    private AuthController()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
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

    public async Task GetRegisterEnabled(Action<bool> SetRegisterEnabled)
    {
        bool registerAvailable;
        await reference.Child("Register").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("Variable not exist");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                registerAvailable = (bool)snapshot.Value;
                SetRegisterEnabled(registerAvailable);
            }

        });
        return;
    }

    public async Task RegisterNewUser(string username, string email, string password, string accountType, Action<string> GetNegativeResultOfUserCreation)
    {
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
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
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = username,
                PhotoUrl = null,
            };
            newUser.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
            });
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            CreateNewUser(username, email, accountType);

        });
        return;

    }

    public async Task LoginUser(string email, string password, Action<string> GetNegativeResultOfUserLogged)
    {
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
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
        });
        return;

    }

    public void CreateNewUser(string username, string email, string accountType)
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

    public async Task SetCurrentUser(string email, Action<string> ConfirmUserLogged)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            await reference.Child("Users").Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
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
                    string json;

                    switch (accountType)
                    {
                        case "Designer":
                            json = snapshot.GetRawJsonValue();
                            SaveDataDesigner designerData = JsonUtility.FromJson<SaveDataDesigner>(json);
                            currentUser = new Designer(designerData);
                            break;
                        case "Player":
                            json = snapshot.GetRawJsonValue();
                            SaveDataPlayer playerData = JsonUtility.FromJson<SaveDataPlayer>(json);
                            currentUser = new Player(playerData);

                            //LOG
                            Globals.logBuffer.Clear();
                            Globals.logBuffer.Append("User: " + currentUser.GetUserName() + "\n");
                            Globals.logBuffer.Append("Email: " + currentUser.GetEmail());
                            Globals.logBuffer.Append("\n");
                            break;
                    }

                    if (currentUser != null)
                    {
                        ConfirmUserLogged(accountType);
                    }

                }

            });
            return;
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
