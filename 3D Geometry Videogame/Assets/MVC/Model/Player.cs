using System;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;

[System.Serializable]
public class Player : User
{

    private List<MissionPlayer> listOfMissions;

    public Player(string username, string email): base (username, email)
    {
    
        listOfMissions = new List<MissionPlayer>();
    }

    public Player(SaveDataPlayer dataPlayer): base (dataPlayer.username, dataPlayer.email)
    {
        listOfMissions = new List<MissionPlayer>();

        foreach (SaveDataMissionPlayer missionPlayerData in dataPlayer.listOfMissions)
        {
            listOfMissions.Add(new MissionPlayer(missionPlayerData));
        }
    }

    public Dictionary<string, int[]> GetAllMissionPlayer()
    {
        Dictionary<string, int[]> dictMission = new Dictionary<string, int[]>();
        foreach(MissionPlayer mission in listOfMissions)
        {
            dictMission[mission.GetMissionName()] = new int[2] { mission.GetInventory(), mission.GetNumberOfFigures() };
        }

        return dictMission;
    }

    public override void WriteUserToLocalJSON()
    {
        SaveDataPlayer savePlayerDataToLocal = new SaveDataPlayer(username, email, listOfMissions);
        File.WriteAllText(Application.persistentDataPath + "/currentuser.json", JsonUtility.ToJson(savePlayerDataToLocal));
    }

    public MissionPlayer GetMissionByName(string mission)
    {
        MissionPlayer currentMission = null;
        foreach(MissionPlayer missionPlayer in listOfMissions)
        {
            if (missionPlayer.GetMissionName() == mission)
            {
                currentMission = missionPlayer;
                break;
            }
        }
        return currentMission;
    }

    public void UpdateMission(MissionPlayer currentMissionPlayer)
    {
        int index = listOfMissions.FindIndex(a => a.GetMissionName() == currentMissionPlayer.GetMissionName());
        if (index != -1) listOfMissions[index] = currentMissionPlayer;

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            UpdateUserToDB(user.UserId);
        }
    }

    public override void WriteNewUserToDB()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            SaveDataPlayer savePlayerDataToDB = new SaveDataPlayer(username, email, listOfMissions);
            reference.Child("Users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(savePlayerDataToDB));
        }
        
    }

    public void AddNewMission(string userId, MissionPlayer missionPlayer)
    {
        listOfMissions.Add(missionPlayer);
        UpdateUserToDB(userId);
    }

    private void UpdateUserToDB(string userId)
    {
        SaveDataPlayer savePlayerDataToDB = new SaveDataPlayer(username, email, listOfMissions);
        reference.Child("Users").Child(userId).SetRawJsonValueAsync(JsonUtility.ToJson(savePlayerDataToDB));
    }

    public void SaveNewChallengeCreator(ChallengeCreator challengeCreator)
    {
        SaveDataChallengeCreator challengeCreatorDataToDB = challengeCreator.WriteToDB();
        reference.Child("Challenges").Child(challengeCreatorDataToDB.missionName).SetRawJsonValueAsync(JsonUtility.ToJson(challengeCreatorDataToDB));
    }
   
}
