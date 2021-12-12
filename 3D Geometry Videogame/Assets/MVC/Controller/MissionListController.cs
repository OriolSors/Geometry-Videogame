using System;
using System.Collections.Generic;
using Firebase.Database;

public sealed class MissionListController
{
    private MissionPlayer currentMissionPlayer = null;

    private MissionListController()
    {
        
    }

    private static MissionListController instance = null;
    public static MissionListController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MissionListController();
            }
            return instance;
        }
    }

    public Dictionary<string, int[]> GetAllMissionPlayer(User currentUser)
    {
        Player currentPlayer = currentUser as Player;
        return currentPlayer.GetAllMissionPlayer();
    }

    public List<string> GetAllMissionDesigner(User currentUser)
    {
        Designer currentDesigner = currentUser as Designer;
        return currentDesigner.GetAllMissionDesigner();
    }

    public Dictionary<string, string> GetAllUserStatisticsInMission(User currentUser, string mission)
    {
        Designer currentDesigner = currentUser as Designer;
        return currentDesigner.GetAllUserStatisticsInMission(mission);
    }

    public void SaveCurrentMissionPlayer(string mission)
    {
        currentMissionPlayer = (AuthController.Instance.GetCurrentUser() as Player).GetMissionByName(mission);
    }

    public MissionPlayer GetCurrentMissionPlayer()
    {
        return currentMissionPlayer;
    }

    public void UpdateMissionPlayer()
    {
        (AuthController.Instance.GetCurrentUser() as Player).UpdateMission(currentMissionPlayer);
    }

    
}
