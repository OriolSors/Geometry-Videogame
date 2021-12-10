using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionPlayer
{
    private string missionName;
    private int numberOfFigures;
    private List<string> characteristics;
    private int inventory;
    private Tatami tatamiGame;
    private Football footballGame;

    public MissionPlayer(string missionName, int numberOfFigures, List<string> characteristics, int inventory, Tatami tatamiGame, Football footballGame)
    {
        this.missionName = missionName;
        this.numberOfFigures = numberOfFigures;
        this.characteristics = characteristics;
        this.inventory = inventory;
        this.tatamiGame = tatamiGame;
        this.footballGame = footballGame;
    }

    public MissionPlayer(SaveDataMissionPlayer missionPlayerData)
    {
        this.missionName = missionPlayerData.missionName;
        this.numberOfFigures = missionPlayerData.numberOfFigures;
        this.characteristics = missionPlayerData.characteristics;
        this.inventory = missionPlayerData.inventory;
        this.tatamiGame = new Tatami(missionPlayerData.tatamiGame);
        this.footballGame = new Football(missionPlayerData.footballGame);
    }

    public string GetMissionName()
    {
        return missionName;
    }

    public int GetNumberOfFigures()
    {
        return numberOfFigures;
    }

    public int GetInventory()
    {
        return inventory;
    }

    public SaveDataMissionPlayer WriteToDB(string player)
    {
        SaveDataMissionPlayer missionPlayerToDB = new SaveDataMissionPlayer(player, missionName, numberOfFigures, characteristics, inventory, tatamiGame, footballGame);
        return missionPlayerToDB;
    }
}
