using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChallengeCreator 
{
    private string missionName;
    private string designerOfMission;
    private int numberOfFigures;
    private List<Vector3> cubePositions;
    private Dictionary<string, ChallengePlayer> listOfPlayers = new Dictionary<string, ChallengePlayer>();
    private int likes = 0;
    private int dislikes = 0;
    private float ratioLikesDislikes = 0;
    private float averageTimeCompleted = 0;

    public ChallengeCreator(string missionName, string designerOfMission, int numberOfFigures, List<Vector3> cubePositions, Dictionary<string, ChallengePlayer> listOfPlayers)
    {
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        this.cubePositions = cubePositions;
        this.listOfPlayers = listOfPlayers;
    }

    public ChallengeCreator(SaveDataChallengeCreator missionPlayerCreatorData)
    {
        this.missionName = missionPlayerCreatorData.missionName;
        this.designerOfMission = missionPlayerCreatorData.designerOfMission;
        this.numberOfFigures = missionPlayerCreatorData.numberOfFigures;
        this.cubePositions = missionPlayerCreatorData.cubePositions;
        this.listOfPlayers = new Dictionary<string, ChallengePlayer>();
        this.averageTimeCompleted = 0;
        foreach (SaveDataChallengePlayer missionPlayerData in missionPlayerCreatorData.listOfPlayers)
        {
            this.listOfPlayers[missionPlayerData.playerName] = new ChallengePlayer(missionPlayerData);
            if (missionPlayerData.like) this.likes += 1;
            else this.dislikes += 1;
            averageTimeCompleted += missionPlayerData.timeToComplete;
        }
        this.ratioLikesDislikes = likes / (likes + dislikes);
        if (listOfPlayers.Keys.Count != 0) this.averageTimeCompleted = averageTimeCompleted / listOfPlayers.Keys.Count;
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
    public float GetLikes()
    {
        return this.likes;
    }
    public float GetDislikes()
    {
        return this.dislikes;
    }
    public float GetRatioLikesDislikes()
    {
        return this.ratioLikesDislikes;
    }

    public SaveDataChallengeCreator WriteToDB()
    {
        SaveDataChallengeCreator challengeCreatorToDB = new SaveDataChallengeCreator(missionName, designerOfMission, numberOfFigures, cubePositions, listOfPlayers, likes, dislikes, GetRatioLikesDislikes(),averageTimeCompleted);
        return challengeCreatorToDB;
    }
}
