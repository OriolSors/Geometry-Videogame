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
        foreach(SaveDataMissionPlayer missionPlayerData in dataPlayer.listOfMissions)
        {
            listOfMissions = new List<MissionPlayer>();
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

    public override void SetAllMissions(IEnumerable<DataSnapshot> listOfMissions)
    {
        foreach(DataSnapshot missionPlayer in listOfMissions)
        {
            MissionPlayer newMissionPlayer = missionPlayer.Value as MissionPlayer;

            /*
            string missionName = missionPlayer.Child("missionName").Value.ToString();

            int numberOfFigures = Convert.ToInt32(missionPlayer.Child("missionName").Value);

            List<string> characteristics = new();
            foreach(DataSnapshot characteristic in missionPlayer.Child("characteristics").Children)
            {
                characteristics.Add(characteristic.Value.ToString());
            }

            int inventory = Convert.ToInt32(missionPlayer.Child("inventory").Value);

            Tatami tatamiGame = new Tatami(Convert.ToInt32(missionPlayer.Child("tatamiGame").Child("currentWave").Value),
                                            missionPlayer.Child("tatamiGame").Child("isFigureCollectedInWave").Value as Dictionary<int, bool>);
            Football footballGame = new Football(Convert.ToInt32(missionPlayer.Child("footballGame").Child("currentWave").Value),
                                            missionPlayer.Child("footballGame").Child("isFigureCollectedInWave").Value as Dictionary<int, bool>);

            MissionPlayer newMissionPlayer = new MissionPlayer(missionName, numberOfFigures, characteristics, inventory, tatamiGame, footballGame);

            */

            this.listOfMissions.Add(newMissionPlayer);
        }
    }
}
