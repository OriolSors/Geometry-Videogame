using System;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;

[System.Serializable]
public class Player : User
{

    private List<MissionPlayer> listOfMissions;

    private List<ChallengePlayer> listOfChallenges;

    public Player(string username, string email): base (username, email)
    {
    
        listOfMissions = new List<MissionPlayer>();
        listOfChallenges = new List<ChallengePlayer>();
    }

    public Player(SaveDataPlayer dataPlayer): base (dataPlayer.username, dataPlayer.email)
    {
        listOfMissions = new List<MissionPlayer>();
        listOfChallenges = new List<ChallengePlayer>();

        foreach (SaveDataMissionPlayer missionPlayerData in dataPlayer.listOfMissions)
        {
            listOfMissions.Add(new MissionPlayer(missionPlayerData));
        }

        foreach (SaveDataChallengePlayer challengePlayerData in dataPlayer.listOfChallenges)
        {
            listOfChallenges.Add(new ChallengePlayer(challengePlayerData));
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

    public Dictionary<string, string> GetAllChallengePlayer()
    {
        Dictionary<string,string> challengesList = new Dictionary<string,string>();
        foreach (ChallengePlayer challenge in listOfChallenges)
        {
            if (challenge.IsCompleted())
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(challenge.GetTimeCompleted());
                string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                challengesList[challenge.GetMissionName()] = timeText;
            }
            else
            {
                challengesList[challenge.GetMissionName()] = "N/A";
            }
        }

        return challengesList;
    }


    public override void WriteUserToLocalJSON()
    {
        SaveDataPlayer savePlayerDataToLocal = new SaveDataPlayer(username, email, listOfMissions, listOfChallenges);
        File.WriteAllText(Application.persistentDataPath + "/currentuser.json", JsonUtility.ToJson(savePlayerDataToLocal));
    }

    public ChallengePlayer GetChallengeByName(string mission)
    {
        ChallengePlayer currentChallenge = null;
        foreach (ChallengePlayer challengePlayer in listOfChallenges)
        {
            if (challengePlayer.GetMissionName() == mission)
            {
                currentChallenge = challengePlayer;
                break;
            }
        }
        return currentChallenge;
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

    public void UpdateChallenge(ChallengePlayer currentChallengePlayer)
    {
        int index = listOfChallenges.FindIndex(a => a.GetMissionName() == currentChallengePlayer.GetMissionName());
        if (index != -1) listOfChallenges[index] = currentChallengePlayer;

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            UpdateUserToDB(user.UserId);
        }
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
            SaveDataPlayer savePlayerDataToDB = new SaveDataPlayer(username, email, listOfMissions, listOfChallenges);
            reference.Child("Users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(savePlayerDataToDB));
        }
        
    }

    public void AddNewMission(string userId, MissionPlayer missionPlayer)
    {
        listOfMissions.Add(missionPlayer);
        UpdateUserToDB(userId);
    }

    public void AddNewChallengePlayer(string userId, ChallengePlayer challengePlayer)
    {
        listOfChallenges.Add(challengePlayer);
        UpdateUserToDB(userId);
    }

    private void UpdateUserToDB(string userId)
    {
        SaveDataPlayer savePlayerDataToDB = new SaveDataPlayer(username, email, listOfMissions, listOfChallenges);
        reference.Child("Users").Child(userId).SetRawJsonValueAsync(JsonUtility.ToJson(savePlayerDataToDB));
    }

    public void SaveNewChallengeCreator(ChallengeCreator challengeCreator)
    {
        SaveDataChallengeCreator challengeCreatorDataToDB = challengeCreator.WriteToDB();
        reference.Child("Challenges").Child(challengeCreatorDataToDB.missionName).SetRawJsonValueAsync(JsonUtility.ToJson(challengeCreatorDataToDB));
    }

    

}
