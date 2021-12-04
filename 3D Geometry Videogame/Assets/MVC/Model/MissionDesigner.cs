using System.Collections;
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
}
