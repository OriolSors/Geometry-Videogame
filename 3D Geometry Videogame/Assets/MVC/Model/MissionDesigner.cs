using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionDesigner
{
    private string missionName;
    private int numberOfFigures;
    private Dictionary<string, MissionPlayer> listOfPlayers;

    public MissionDesigner(string missionName, int numberOfFigures, Dictionary<string, MissionPlayer> listOfPlayers)
    {
        this.missionName = missionName;
        this.numberOfFigures = numberOfFigures;
        this.listOfPlayers = listOfPlayers;
    }

    public MissionDesigner(SaveDataMissionDesigner missionDesignerData)
    {
        this.missionName = missionDesignerData.missionName;
        this.numberOfFigures = missionDesignerData.numberOfFigures;
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

    public SaveDataMissionDesigner WriteToDB()
    {
        SaveDataMissionDesigner missionDesignerToDB = new SaveDataMissionDesigner(missionName, numberOfFigures, listOfPlayers);
        return missionDesignerToDB;
    }
}
