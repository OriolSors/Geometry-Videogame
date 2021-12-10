using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveDataPlayer
{
    [SerializeField]
    private string username;

    [SerializeField]
    private string account;

    [SerializeField]
    private List<SaveDataMissionPlayer> listOfMissions;

    public SaveDataPlayer(string username, List<MissionPlayer> listOfMissions)
    {
        this.username = username;
        account = "Player";
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
    private string username;

    [SerializeField]
    private string account;

    [SerializeField]
    private List<SaveDataMissionDesigner> listOfMissions = new List<SaveDataMissionDesigner>();

    public SaveDataDesigner(string username, List<MissionDesigner> listOfMissions)
    {
        this.username = username;
        account = "Designer";

        foreach(MissionDesigner mission in listOfMissions)
        {
            this.listOfMissions.Add(mission.WriteToDB());
        }
        
    }
}

[System.Serializable]
public class SaveDataMissionDesigner
{
    [SerializeField]
    private string missionName;

    [SerializeField]
    private int numberOfFigures;

    [SerializeField]
    private List<SaveDataMissionPlayer> listOfPlayers = new List<SaveDataMissionPlayer>();

    public SaveDataMissionDesigner(string missionName, int numberOfFigures, Dictionary<string, MissionPlayer> listOfPlayers)
    {
        this.missionName = missionName;
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
    private string playerName;

    [SerializeField]
    private string missionName;

    [SerializeField]
    private int numberOfFigures;

    [SerializeField]
    private List<string> characteristics;

    [SerializeField]
    private int inventory;

    [SerializeField]
    private SaveDataMinigame tatamiGame;

    [SerializeField]
    private SaveDataMinigame footballGame;

    public SaveDataMissionPlayer(string playerName, string missionName, int numberOfFigures, List<string> characteristics, int inventory, Tatami tatamiGame, Football footballGame)
    {
        this.playerName = playerName;
        this.missionName = missionName;
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
    private int currentWave;

    [SerializeField]
    private List<SaveDataFigureInWave> isFigureCollectedInWave = new List<SaveDataFigureInWave>();

    public SaveDataMinigame(int currentWave, Dictionary<int, bool> isFigureCollectedInWave)
    {
        this.currentWave = currentWave;

        foreach(int wave in isFigureCollectedInWave.Keys)
        {
            this.isFigureCollectedInWave.Add(new SaveDataFigureInWave(wave, isFigureCollectedInWave[wave]));
        }

    }

}

[System.Serializable]
public class SaveDataFigureInWave
{
    [SerializeField]
    private int waveNumber;

    [SerializeField]
    private bool isCollected;

    public SaveDataFigureInWave(int waveNumber, bool isCollected)
    {
        this.waveNumber = waveNumber;

        this.isCollected = isCollected;

    }

}

