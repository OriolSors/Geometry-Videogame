using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectController
{
    private List<string> characteristics;
    private Dictionary<string, string> challenges;
    private Dictionary<string, int> figureScore;

    public CollectController()
    {
        characteristics = MissionListController.Instance.GetCurrentMissionPlayer().GetCharacteristics();
    }


    private void SetChallengesAndScores(List<string> prefabNames)
    {
        ChallengesListController challengesList = new ChallengesListController(characteristics, prefabNames);
        challenges = challengesList.GetCurrentChallenges();
        figureScore = challengesList.GetCurrentScores();
    }

    public Dictionary<string, string> ObtainChallenges(List<string> prefabNames)
    {
        SetChallengesAndScores(prefabNames);
        return challenges;
    }

    public Dictionary<string, int> ObtainFigureScore()
    {
        return figureScore;
    }

    public void IncreaseInventory()
    {
        MissionListController.Instance.GetCurrentMissionPlayer().IncreaseInventory();
    }

    public void SaveCurrentMission()
    {
        MissionListController.Instance.UpdateMissionPlayer();
    }
    
}
