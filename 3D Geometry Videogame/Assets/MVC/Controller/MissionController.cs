using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class MissionController
{
    private DatabaseReference reference;
    private UserController userController;

    public MissionController()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        userController = new UserController();
    }

    public void CreateNewMission(string missionName, User designerOfMission, List<Vector3> cubePositions, bool isDefaultMission, Dictionary<string, List<string>> playersDict)
    {
        int numberOfFigures = cubePositions.Count;
        Mission mission = new Mission(missionName, designerOfMission.GetEmail(), numberOfFigures, cubePositions, isDefaultMission);
        mission.CreateMissionDesigner(playersDict);
    }

    public void CreateNewMissionByPlayer(string missionName, User designerOfMission, List<Vector3> cubePositions)
    {
        int numberOfFigures = cubePositions.Count;
        Mission mission = new Mission(missionName, designerOfMission.GetEmail(), numberOfFigures, cubePositions, false);

        Player player = designerOfMission as Player;
        mission.CreateChallengeByPlayer(player.GetLevel());
    }

    public void GetAllPlayerNames(Action<List<string>> callbackListOfPlayerNames)
    {
        userController.GetAllPlayerNames(callbackListOfPlayerNames);
    }

    public bool CheckValidCharacteristics(List<string> characteristics)
    {
        ChallengesListController challengesListController = new ChallengesListController(characteristics);
        return challengesListController.CheckValidCharacteristics();
    }
}
