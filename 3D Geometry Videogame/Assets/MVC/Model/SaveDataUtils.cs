using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveDataPlayer
{
    [SerializeField]
    public string username;

    [SerializeField]
    public string account;

    [SerializeField]
    public string email;

    [SerializeField]
    public List<SaveDataMissionPlayer> listOfMissions = new List<SaveDataMissionPlayer>();

    public SaveDataPlayer(string username, string email, List<MissionPlayer> listOfMissions)
    {
        this.username = username;
        account = "Player";
        this.email = email;
        foreach (MissionPlayer mission in listOfMissions)
        {
            this.listOfMissions.Add(mission.WriteToDB(username));
        }

    }
}

[System.Serializable]
public class SaveDataDesigner
{
    [SerializeField]
    public string username;

    [SerializeField]
    public string account;

    [SerializeField]
    public string email;

    [SerializeField]
    public List<SaveDataMissionDesigner> listOfMissions = new List<SaveDataMissionDesigner>();

    public SaveDataDesigner(string username, string email, List<MissionDesigner> listOfMissions)
    {
        this.username = username;
        account = "Designer";
        this.email = email;
        foreach (MissionDesigner mission in listOfMissions)
        {
            this.listOfMissions.Add(mission.WriteToDB());
        }
        
    }
}

[System.Serializable]
public class SaveDataMissionDesigner
{
    [SerializeField]
    public string missionName;

    [SerializeField]
    public string designerOfMission;

    [SerializeField]
    public int numberOfFigures;

    [SerializeField]
    public List<SaveDataMissionPlayer> listOfPlayers = new List<SaveDataMissionPlayer>();

    public SaveDataMissionDesigner(string missionName, string designerOfMission, int numberOfFigures, Dictionary<string, MissionPlayer> listOfPlayers)
    {
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        foreach (string player in listOfPlayers.Keys)
        {
            this.listOfPlayers.Add(listOfPlayers[player].WriteToDB(player));
        }

    }
}

[System.Serializable]
public class SaveDataMissionPlayer
{
    [SerializeField]
    public string playerName;

    [SerializeField]
    public string missionName;

    [SerializeField]
    public string designerOfMission;

    [SerializeField]
    public int numberOfFigures;

    [SerializeField]
    public List<string> characteristics;

    [SerializeField]
    public int inventory;

    [SerializeField]
    public SaveDataMinigame tatamiGame;

    [SerializeField]
    public SaveDataMinigame footballGame;
    

    public SaveDataMissionPlayer(string playerName, string missionName, string designerOfMission, int numberOfFigures, List<string> characteristics, int inventory, Tatami tatamiGame, Football footballGame)
    {
        this.playerName = playerName;
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        this.characteristics = characteristics;
        this.inventory = inventory;
        this.tatamiGame = tatamiGame.WriteToDB();
        this.footballGame = footballGame.WriteToDB();

    }
}

[System.Serializable]
public class SaveDataMinigame
{
    [SerializeField]
    public int currentWave;

    [SerializeField]
    public List<SaveDataFigureInWave> isFigureCollectedInWave = new List<SaveDataFigureInWave>();

    public SaveDataMinigame(int currentWave, Dictionary<int, bool> isFigureCollectedInWave)
    {
        this.currentWave = currentWave;

        foreach(int wave in isFigureCollectedInWave.Keys)
        {
            this.isFigureCollectedInWave.Add(new SaveDataFigureInWave(wave, isFigureCollectedInWave[wave]));
        }

    }

    public Dictionary<int, bool> FiguresCollectedToDictionary()
    {
        Dictionary<int, bool> isFigureCollectedInWave =  new Dictionary<int, bool>();
        foreach(SaveDataFigureInWave pair in this.isFigureCollectedInWave)
        {
            isFigureCollectedInWave[pair.waveNumber] = pair.isCollected;
        }

        return isFigureCollectedInWave;
    }

}

[System.Serializable]
public class SaveDataFigureInWave
{
    [SerializeField]
    public int waveNumber;

    [SerializeField]
    public bool isCollected;

    public SaveDataFigureInWave(int waveNumber, bool isCollected)
    {
        this.waveNumber = waveNumber;

        this.isCollected = isCollected;

    }

}

