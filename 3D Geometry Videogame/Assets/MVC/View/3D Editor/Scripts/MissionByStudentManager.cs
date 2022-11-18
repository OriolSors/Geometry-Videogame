using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MissionByStudentManager : MonoBehaviour
{
    private List<Vector3> cubePositions;

    public Canvas setMissionNameCanvas;
    public Canvas noValidMissionNameCanvas;

    private MissionController missionController;
    void Start()
    {
        missionController = new MissionController();
        noValidMissionNameCanvas.enabled = false;
        setMissionNameCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(List<Vector3> cubePositions)
    {
        this.cubePositions = cubePositions;
    }

    public void SaveNewMission()
    {
        string missionName = setMissionNameCanvas.GetComponentInChildren<TMP_InputField>().text;
        if (!string.IsNullOrWhiteSpace(missionName))
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            missionName += " " + date;
            missionController.CreateNewMissionByPlayer(missionName, AuthController.Instance.GetCurrentUser(), cubePositions);
            SceneManager.LoadScene("Player Mission List Screen");
        }
        else
        {
            noValidMissionNameCanvas.enabled = true;
            setMissionNameCanvas.enabled = false;
        }

    }

    public void ConfirmNoValidMissionName()
    {
        noValidMissionNameCanvas.enabled = false;
        setMissionNameCanvas.enabled = true;
    }


    public void ExitScreen()
    {
        SceneManager.LoadScene("3D Editor");
    }
}
