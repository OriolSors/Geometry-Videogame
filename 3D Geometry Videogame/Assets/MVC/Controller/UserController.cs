using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class UserController
{

    private DatabaseReference reference;
    private Firebase.Auth.FirebaseAuth auth;

    private Dictionary<string, Player> listOfPlayers;

    public UserController()

    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        listOfPlayers = new Dictionary<string, Player>();
    }

    public async void AddNewMissionPlayer(MissionPlayer missionPlayer, string player)
    {
        await GetAllPlayerObjects();
        AddMissionToPlayer(missionPlayer, player);

    }

    public void AddNewMissionDesigner(MissionDesigner missionDesigner, User user)
    {
        Designer designer = user as Designer;
        designer.AddNewMission(missionDesigner);
    }

    public void AddNewChallengeCreator(ChallengeCreator challengeCreator, User user)
    {
        Player player = user as Player;
        player.SaveNewChallengeCreator(challengeCreator);
    }

    private async Task GetAllPlayerObjects()
    {
        Dictionary<string, Player> players = new Dictionary<string, Player>();
        await reference.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
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
                        string json = s.GetRawJsonValue();
                        SaveDataPlayer playerData = JsonUtility.FromJson<SaveDataPlayer>(json);
                        players[s.Key] = new Player(playerData);
                    }
                }
                listOfPlayers = players;
            }
            
        });

        return;
    }

    public async void ReplaceDesignerMission(MissionPlayer currentMissionPlayer)
    {
        Dictionary<string, Designer> missionDesigner = await GetDesignerByEmail(currentMissionPlayer.GetDesigner());
        missionDesigner[missionDesigner.Keys.First()].UpdateMission(auth.CurrentUser.DisplayName,missionDesigner.Keys.First(), currentMissionPlayer);
    }

    public async Task<Dictionary<string,Designer>> GetDesignerByEmail(string designerEmail)
    {
        Dictionary<string, Designer> missionDesigner = new Dictionary<string, Designer>();
        await reference.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
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
                    if (s.Child("account").Value.ToString() == "Designer" && s.Child("email").Value.ToString() == designerEmail)
                    {
                        string json = s.GetRawJsonValue();
                        SaveDataDesigner designerData = JsonUtility.FromJson<SaveDataDesigner>(json);
                        missionDesigner[s.Key] = new Designer(designerData);
                        break;
                    }
                }
                
            }

        });

        return missionDesigner;
    }

    private void AddMissionToPlayer(MissionPlayer missionPlayer, string player)
    {

        foreach (string userId in listOfPlayers.Keys)
        {
            if (listOfPlayers[userId].GetUserName() == player)
            {
                listOfPlayers[userId].AddNewMission(userId, missionPlayer);
            }
        }
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
