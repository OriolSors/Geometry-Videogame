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
    private List<MissionPlayer> listOfMissions;

    public SaveDataPlayer(string username, List<MissionPlayer> listOfMissions)
    {
        this.username = username;
        account = "Player";
        this.listOfMissions = listOfMissions;

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
    private List<MissionDesigner> listOfMissions;

    public SaveDataDesigner(string username, List<MissionDesigner> listOfMissions)
    {
        this.username = username;
        account = "Designer";
        this.listOfMissions = listOfMissions;
    }
}

