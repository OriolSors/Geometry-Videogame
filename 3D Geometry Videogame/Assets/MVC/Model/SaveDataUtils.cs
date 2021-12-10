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
    private List<string> listOfMissions;

    public SaveDataPlayer(string username, List<MissionPlayer> listOfMissions)
    {
        this.username = username;
        account = "Player";
        foreach (MissionPlayer mission in listOfMissions)
        {
            this.listOfMissions.Add(mission.WriteToDB());
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
    private List<string> listOfMissions = new List<string>();

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
    private Dictionary<string, string> listOfPlayers = new Dictionary<string, string>();

    public SaveDataMissionDesigner(string missionName, int numberOfFigures, Dictionary<string, MissionPlayer> listOfPlayers)
    {
        this.missionName = missionName;
        this.numberOfFigures = numberOfFigures;
        foreach (string player in listOfPlayers.Keys)
        {
            this.listOfPlayers[player] = listOfPlayers[player].WriteToDB();
        }

    }
}

[System.Serializable]
public class SaveDataMissionPlayer
{
    [SerializeField]
    private string missionName;

    [SerializeField]
    private int numberOfFigures;

    [SerializeField]
    private List<string> characteristics;

    [SerializeField]
    private int inventory;

    [SerializeField]
    private string tatamiGame;

    [SerializeField]
    private string footballGame;

    public SaveDataMissionPlayer(string missionName, int numberOfFigures, List<string> characteristics, int inventory, Tatami tatamiGame, Football footballGame)
    {
        this.missionName = missionName;
        this.numberOfFigures = numberOfFigures;
        this.characteristics = characteristics;
        this.inventory = inventory;
        this.tatamiGame = JsonUtility.ToJson(tatamiGame);
        this.footballGame = JsonUtility.ToJson(footballGame);

    }
}

