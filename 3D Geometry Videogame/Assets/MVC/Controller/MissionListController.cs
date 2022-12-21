using System;
using System.Collections.Generic;
using Firebase.Database;

public sealed class MissionListController
{
    private MissionPlayer currentMissionPlayer = null;
    private ChallengePlayer currentChallengePlayer = null;

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

    public Dictionary<string, string> GetAllChallengePlayer(User currentUser)
    {
        Player currentPlayer = currentUser as Player;
        return currentPlayer.GetAllChallengePlayer();
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

    public void SaveCurrentChallengePlayer(string mission)
    {
        currentChallengePlayer = (AuthController.Instance.GetCurrentUser() as Player).GetChallengeByName(mission);

        //LOG
        Globals.logBuffer.Append("Current Challenge: " + currentChallengePlayer.GetMissionName() + "\n");
        Globals.logBuffer.Append("Designer of the Challenge: " + currentChallengePlayer.GetDesigner() + "\n");
        Globals.logBuffer.Append("Number of figures: " + currentChallengePlayer.GetNumberOfFigures() + "\n");
        Globals.logBuffer.Append("\n");
    }

    public MissionPlayer GetCurrentMissionPlayer()
    {
        return currentMissionPlayer;
    }

    public ChallengePlayer GetCurrentChallengePlayer()
    {
        return currentChallengePlayer;
    }

    public void UpdateMissionPlayer()
    {
        (AuthController.Instance.GetCurrentUser() as Player).UpdateMission(currentMissionPlayer);
        UserController userController = new UserController();
        userController.ReplaceDesignerMission(currentMissionPlayer);
    }

    public async void UpdateChallengePlayer()
    {
        (AuthController.Instance.GetCurrentUser() as Player).UpdateChallenge(currentChallengePlayer);
        UserController userController = new UserController();
        await userController.GetAllPlayerObjects();
        userController.ReplaceCreatorChallenge(currentChallengePlayer);
    }


}
