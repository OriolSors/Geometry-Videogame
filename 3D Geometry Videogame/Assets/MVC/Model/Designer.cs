using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;

[System.Serializable]
public class Designer : User
{
    private List<MissionDesigner> listOfMissionsDesigned;

    public Designer(string username, string email): base (username, email)
    {
        listOfMissionsDesigned = new List<MissionDesigner>();
    }

    public Designer(SaveDataDesigner dataDesigner) : base(dataDesigner.username, dataDesigner.email)
    {
        listOfMissionsDesigned = new List<MissionDesigner>();

        foreach (SaveDataMissionDesigner missionDesignerData in dataDesigner.listOfMissions)
        {
            listOfMissionsDesigned.Add(new MissionDesigner(missionDesignerData));
        }
    }

    public List<MissionDesigner> GetMissionDesigner()
    {
        return listOfMissionsDesigned;
    }

    public override void WriteUserToLocalJSON()
    {
        SaveDataDesigner saveDesignerDataToLocal = new SaveDataDesigner(username, email, listOfMissionsDesigned);
        File.WriteAllText(Application.persistentDataPath + "/currentuser.json", JsonUtility.ToJson(saveDesignerDataToLocal));
    }

    public override void WriteNewUserToDB()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            SaveDataDesigner saveDesignerDataToDB = new SaveDataDesigner(username, email, listOfMissionsDesigned);
            reference.Child("Users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(saveDesignerDataToDB));
        }
    }

    private void UpdateUserToDB()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            SaveDataDesigner saveDesignerDataToDB = new SaveDataDesigner(username, email, listOfMissionsDesigned);
            reference.Child("Users").Child(user.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(saveDesignerDataToDB));
        }
    }

    public void AddNewMission(MissionDesigner missionDesigner)
    {
        listOfMissionsDesigned.Add(missionDesigner);
        UpdateUserToDB();
    }

    public List<string> GetAllMissionDesigner()
    {
        List<string> missions = new List<string>();
        foreach(MissionDesigner mission in listOfMissionsDesigned)
        {
            missions.Add(mission.GetMissionName());
        }

        return missions;
    }

    public Dictionary<string, string> GetAllUserStatisticsInMission(string currentMission)
    {
        Dictionary<string, string> userStatistics = new Dictionary<string, string>();
        foreach(MissionDesigner mission in listOfMissionsDesigned)
        {
            if (mission.GetMissionName() == currentMission)
            {
                Dictionary<string, MissionPlayer> players = mission.GetListOfPlayers();

                if (players != null)
                {
                    foreach (string player in players.Keys)
                    {
                        if (players[player].GetInventory() >= players[player].GetNumberOfFigures())
                        {
                            userStatistics[player] = "100%";
                        }
                        else
                        {
                            userStatistics[player] = (players[player].GetInventory() / players[player].GetNumberOfFigures()).ToString() + "%";
                        }
                    }
                }
                
                break;
            }
        }

        return userStatistics;
    }

    
}
