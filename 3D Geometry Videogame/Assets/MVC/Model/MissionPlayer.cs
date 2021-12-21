using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionPlayer
{
    private string missionName;
    private string designerOfMission;
    private int numberOfFigures;
    private List<Vector3> cubePositions;
    private List<string> characteristics;
    private int inventory;
    private Tatami tatamiGame;
    private Football footballGame;

    public MissionPlayer(string missionName, string designerOfMission, int numberOfFigures, List<Vector3>cubePositions, List<string> characteristics, int inventory, Tatami tatamiGame, Football footballGame)
    {
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        this.cubePositions = cubePositions;
        this.characteristics = characteristics;
        this.inventory = inventory;
        this.tatamiGame = tatamiGame;
        this.footballGame = footballGame;
    }

    public MissionPlayer(SaveDataMissionPlayer missionPlayerData)
    {
        this.missionName = missionPlayerData.missionName;
        this.designerOfMission = missionPlayerData.designerOfMission;
        this.numberOfFigures = missionPlayerData.numberOfFigures;
        this.cubePositions = missionPlayerData.cubePositions;
        this.characteristics = missionPlayerData.characteristics;
        this.inventory = missionPlayerData.inventory;
        this.tatamiGame = new Tatami(missionPlayerData.tatamiGame);
        this.footballGame = new Football(missionPlayerData.footballGame);
    }

    public List<string> GetCharacteristics()
    {
        return characteristics;
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

    public int GetInventory()
    {
        return inventory;
    }

    public SaveDataMissionPlayer WriteToDB(string player)
    {
        SaveDataMissionPlayer missionPlayerToDB = new SaveDataMissionPlayer(player, missionName, designerOfMission, numberOfFigures, cubePositions, characteristics, inventory, tatamiGame, footballGame);
        return missionPlayerToDB;
    }

    public void IncreaseInventory()
    {
        inventory++;
    }

    public Tatami GetTatami()
    {
        return tatamiGame;
    }

    public Football GetFootball()
    {
        return footballGame;
    }
}
