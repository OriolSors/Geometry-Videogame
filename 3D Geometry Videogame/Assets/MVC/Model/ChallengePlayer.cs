using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChallengePlayer
{
    private string missionName;
    private string designerOfMission;
    private int numberOfFigures;
    private List<Vector3> cubePositions;
    private float timeToComplete;
    private bool completed = false;
    private bool like;


    public ChallengePlayer(string missionName, string designerOfMission, int numberOfFigures, List<Vector3> cubePositions)
    {
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        this.cubePositions = cubePositions;

    }

    public ChallengePlayer(SaveDataChallengePlayer missionPlayerData)
    {
        this.missionName = missionPlayerData.missionName;
        this.designerOfMission = missionPlayerData.designerOfMission;
        this.numberOfFigures = missionPlayerData.numberOfFigures;
        this.cubePositions = missionPlayerData.cubePositions;
        this.timeToComplete = missionPlayerData.timeToComplete;
        this.completed = missionPlayerData.completed;
        this.like = missionPlayerData.like;
    }

    public string GetMissionName()
    {
        return missionName;
    }

    public string GetDesigner()
    {
        return designerOfMission;
    }

    public int GetNumberOfFigures()
    {
        return numberOfFigures;
    }

    public List<Vector3> GetCubePositions()
    {
        return cubePositions;
    }

    public float GetTimeCompleted()
    {
        return timeToComplete;
    }
    public bool IsCompleted()
    {
        return completed;
    }

    public void SetTimeCompleted(int seconds)
    {
        completed = true;
        timeToComplete = seconds;
    }

    public void SetLike(bool like)
    {
        this.like = like;
    }

    public bool GetLike()
    {
        return like;
    }

    public SaveDataChallengePlayer WriteToDB(string player)
    {
        SaveDataChallengePlayer challengePlayerToDB = new SaveDataChallengePlayer(player, missionName, designerOfMission, numberOfFigures, cubePositions, timeToComplete, completed, like);
        return challengePlayerToDB;
    }

    
}
