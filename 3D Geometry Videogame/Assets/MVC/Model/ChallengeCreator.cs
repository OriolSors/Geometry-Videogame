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
    private int[] ratioLikesDislikes = new int[2] { 0, 0 };

    public ChallengeCreator(string missionName, string designerOfMission, int numberOfFigures, List<Vector3> cubePositions)
    {
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        this.cubePositions = cubePositions;

    }

    public ChallengeCreator(SaveDataChallengeCreator missionPlayerCreatorData)
    {
        this.missionName = missionPlayerCreatorData.missionName;
        this.designerOfMission = missionPlayerCreatorData.designerOfMission;
        this.numberOfFigures = missionPlayerCreatorData.numberOfFigures;
        this.cubePositions = missionPlayerCreatorData.cubePositions;
        this.listOfPlayers = new Dictionary<string, ChallengePlayer>();
        foreach (SaveDataChallengePlayer missionPlayerData in missionPlayerCreatorData.listOfPlayers)
        {
            this.listOfPlayers[missionPlayerData.playerName] = new ChallengePlayer(missionPlayerData);
            if (missionPlayerData.like) this.ratioLikesDislikes[0] += 1;
            else this.ratioLikesDislikes[1] += 1;
        }
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
        return this.ratioLikesDislikes[0];
    }
    public float GetDislikes()
    {
        return this.ratioLikesDislikes[1];
    }
    public float GetRatioLikesDislikes()
    {
        return this.ratioLikesDislikes[0] / this.ratioLikesDislikes[1];
    }

    public SaveDataChallengeCreator WriteToDB()
    {
        SaveDataChallengeCreator challengeCreatorToDB = new SaveDataChallengeCreator(missionName, designerOfMission, numberOfFigures, cubePositions, listOfPlayers);
        return challengeCreatorToDB;
    }
}
