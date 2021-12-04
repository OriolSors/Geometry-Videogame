using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission
{
    private string missionName;
    private string designerOfMission;
    private int numberOfFigures;
    private bool isDefaultMission;

    public Mission(string missionName, string designerOfMission, int numberOfFigures, bool isDefaultMission)
    {
        this.missionName = missionName;
        this.designerOfMission = designerOfMission;
        this.numberOfFigures = numberOfFigures;
        this.isDefaultMission = isDefaultMission;
    }


    public void CreateMissionPlayer()
    {

    }

    public void CreateMissionDesigner()
    {

    }
}
