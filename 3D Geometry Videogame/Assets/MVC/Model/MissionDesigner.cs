using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionDesigner
{
    private string missionName;
    private string designerOfMission;
    private int numberOfFigures;
    private List<Vector3> cubePositions;
    private Dictionary<string, MissionPlayer> listOfPlayers;

    public MissionDesigner(string missionName, string designerOfMission, int numberOfFigures, List<Vector3> cubePositions, Dictionary<string, MissionPlayer> listOfPlayers)
    {
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        this.cubePositions = cubePositions;
        this.listOfPlayers = listOfPlayers;
    }

    public MissionDesigner(SaveDataMissionDesigner missionDesignerData)
    {
        this.missionName = missionDesignerData.missionName;
        this.designerOfMission = missionDesignerData.designerOfMission;
        this.numberOfFigures = missionDesignerData.numberOfFigures;
        this.cubePositions = missionDesignerData.cubePositions;
        this.listOfPlayers = new Dictionary<string, MissionPlayer>();
        foreach(SaveDataMissionPlayer missionPlayerData in missionDesignerData.listOfPlayers)
        {
            this.listOfPlayers[missionPlayerData.playerName] = new MissionPlayer(missionPlayerData);
        }
    }

    public string GetMissionName()
    {
        return missionName;
    }

    public int GetNumberOfFigures()
    {
        return numberOfFigures;
    }

    public Dictionary<string, MissionPlayer> GetListOfPlayers()
    {
        return listOfPlayers;
    }

    public void SetListOfPlayers(Dictionary<string, MissionPlayer> listOfPlayers)
    {
        this.listOfPlayers = listOfPlayers;
    }

    public SaveDataMissionDesigner WriteToDB()
    {
        SaveDataMissionDesigner missionDesignerToDB = new SaveDataMissionDesigner(missionName, designerOfMission, numberOfFigures, cubePositions, listOfPlayers);
        return missionDesignerToDB;
    }
}
