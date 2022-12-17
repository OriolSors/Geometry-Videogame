using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission
{
    private string missionName;
    private string designerOfMission;
    private int numberOfFigures;
    private List<Vector3> cubePositions;
    private bool isDefaultMission;

    public Mission(string missionName, string designerOfMission, int numberOfFigures, List<Vector3>cubePositions, bool isDefaultMission)
    {
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        this.cubePositions = cubePositions;
        this.isDefaultMission = isDefaultMission;
    }


    public Dictionary<string, MissionPlayer> CreateMissionPlayer(Dictionary<string, List<string>> playersDict)
    {
        Dictionary<string, MissionPlayer> listOfPlayers = new Dictionary<string, MissionPlayer>();
        foreach(string player in playersDict.Keys)
        {
            MissionPlayer missionPlayer = new MissionPlayer(missionName, designerOfMission, numberOfFigures, cubePositions, playersDict[player], 0,
                new Tatami(0, new Dictionary<int, bool>(), numberOfFigures), new Football(0, new Dictionary<int, bool>(), numberOfFigures));

            listOfPlayers[player] = missionPlayer;

            UserController userController = new UserController();
            userController.AddNewMissionPlayer(missionPlayer, player);
        }

        return listOfPlayers;
    }

    public void CreateMissionDesigner(Dictionary<string, List<string>> playersDict)
    {
        MissionDesigner missionDesigner = new MissionDesigner(missionName, designerOfMission, numberOfFigures, cubePositions, CreateMissionPlayer(playersDict));
        UserController userController = new UserController();
        userController.AddNewMissionDesigner(missionDesigner, AuthController.Instance.GetCurrentUser());
    }

    public Dictionary<string, ChallengePlayer> CreateChallengePlayer(Dictionary<string, Player> playersDict)
    {
        Dictionary<string, ChallengePlayer> listOfPlayers = new Dictionary<string, ChallengePlayer>();
        foreach (string playerId in playersDict.Keys)
        {
            string player = playersDict[playerId].GetEmail();
            if(designerOfMission != player)
            {
                ChallengePlayer challengePlayer = new ChallengePlayer(missionName, designerOfMission, numberOfFigures, cubePositions);

                listOfPlayers[playersDict[playerId].GetUserName()] = challengePlayer;

                UserController userController = new UserController();
                userController.AddNewChallengePlayer(challengePlayer, playersDict[playerId]);
            }
            
        }

        return listOfPlayers;
    }

    public async void CreateChallengeByPlayer()
    {

        //TODO: Decidir a quins alumnes s'assignaràn aquestes missions (??)
        UserController userController = new UserController();
        await userController.GetAllPlayerObjects();
        ChallengeCreator challengeCreator = new ChallengeCreator(missionName, designerOfMission, numberOfFigures, cubePositions, CreateChallengePlayer(userController.GetPlayers())); // PASSAR LA LLISTA DE TOTS ELS USERS DESTINATS AL CHALLENGE
        
        userController.AddNewChallengeCreator(challengeCreator, AuthController.Instance.GetCurrentUser());
    }
}
