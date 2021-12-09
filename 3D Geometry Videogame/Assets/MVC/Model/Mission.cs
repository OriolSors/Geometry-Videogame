using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission
{
    private string missionName;
    private string designerOfMission;
    private int numberOfFigures;
    private bool isDefaultMission;

    public Mission(string missionName, string designerOfMission, int numberOfFigures, bool isDefaultMission)
    {
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        this.isDefaultMission = isDefaultMission;
    }


    public Dictionary<string, MissionPlayer> CreateMissionPlayer(Dictionary<string, List<string>> playersDict)
    {
        Dictionary<string, MissionPlayer> listOfPlayers = new Dictionary<string, MissionPlayer>();
        foreach(string player in playersDict.Keys)
        {
            MissionPlayer missionPlayer = new MissionPlayer(missionName, numberOfFigures, playersDict[player], 0,
                new Tatami(0, new Dictionary<int, bool>(), numberOfFigures), new Football(0, new Dictionary<int, bool>(), numberOfFigures));

            listOfPlayers[player] = missionPlayer;

            UserController userController = new UserController();
            userController.AddNewMissionPlayer(missionPlayer, player);
        }

        return listOfPlayers;
    }

    public void CreateMissionDesigner(Dictionary<string, List<string>> playersDict)
    {
        MissionDesigner missionDesigner = new MissionDesigner(missionName, numberOfFigures, CreateMissionPlayer(playersDict));
        UserController userController = new UserController();
        userController.AddNewMissionDesigner(missionDesigner, AuthController.Instance.GetCurrentUser());
    }
}
