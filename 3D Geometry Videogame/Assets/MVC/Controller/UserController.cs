using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class UserController
{

    private DatabaseReference reference;

    Dictionary <string, Player> listOfPlayers;

    public UserController()

    {
        listOfPlayers = new Dictionary<string, Player>();
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
    }

    public void AddNewMissionPlayer(MissionPlayer missionPlayer, string player)
    {
        GetAllPlayerObjects(FillListOfPlayers);

        foreach(string userId in listOfPlayers.Keys)
        {
            if (listOfPlayers[userId].GetUserName() == player)
            {
                listOfPlayers[userId].AddNewMission(userId, missionPlayer);
            }
        }
    }

    public void AddNewMissionDesigner(MissionDesigner missionDesigner, User user)
    {
        Designer designer = user as Designer;
        designer.AddNewMission(missionDesigner);
    }

    private void GetAllPlayerObjects(Action<Dictionary<string, Player>> callbackListOfPlayerObjects)
    {
        Dictionary<string, Player> players = new Dictionary<string, Player>();
        var DBTask = reference.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("No players");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot s in snapshot.Children)
                {
                    if (s.Child("account").Value.ToString() == "Player")
                    {
                        players[s.Key] = s.Value as Player;
                    }
                }
            }

            callbackListOfPlayerObjects(players);
        });
    }

    private void FillListOfPlayers(Dictionary<string, Player> listOfPlayers)
    {
        this.listOfPlayers = listOfPlayers;
    }

    public void GetAllPlayerNames(Action<List<string>> callbackListOfPlayerNames)
    {
        List<string> players = new List<string>();
        var DBTask = reference.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("No players");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot s in snapshot.Children)
                {
                    if (s.Child("account").Value.ToString() == "Player")
                    {
                        players.Add(s.Child("username").Value.ToString());
                    }
                }
            }

            callbackListOfPlayerNames(players);
        });
    }
}
