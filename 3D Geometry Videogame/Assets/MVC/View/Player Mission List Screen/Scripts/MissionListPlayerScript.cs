using System;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MissionListPlayerScript : MonoBehaviour
{
    public RectTransform missionsScroll;
    public GameObject missionView;
    public GameObject missionReadyView;

    public Canvas inventoryCanvas;
    public TextMeshProUGUI inventoryMissionTitle;
    public RectTransform inventoryMissionsScroll;
    public GameObject inventoryMissionView;
    public TextMeshProUGUI inventoryCubeNumbers;
    public TextMeshProUGUI inventoryMissionStatus;

    private MissionListController missionListController;

    void Start()
    {
        missionListController = new MissionListController();
        FillMissionScroll();
        inventoryCanvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab)) inventoryCanvas.enabled = true;
        else inventoryCanvas.enabled = false;
    }

    private void FillMissionScroll()
    {
        Dictionary<string, int[]> missions = missionListController.GetAllMissionPlayer(AuthController.Instance.GetCurrentUser());

        foreach (string mission in missions.Keys)
        {
            
            if (missions[mission][0] >= missions[mission][1])
            {
                //---------- MAIN LIST MISSIONS ----------

                GameObject go = Instantiate(missionReadyView);
                Button minigameMission = go.transform.Find("Mission Button").GetComponent<Button>();
                Button finishMission = go.transform.Find("Ready Button").GetComponent<Button>();

                minigameMission.GetComponentInChildren<Text>().text = mission;
                minigameMission.onClick.AddListener(delegate { LoadMinigames(mission, missions[mission][0]); });
                finishMission.onClick.AddListener(delegate { GoToConstruct(); });

                go.transform.SetParent(missionsScroll);

                //---------- INVENTORY LIST MISSIONS ----------

                GameObject inventoryGo = Instantiate(inventoryMissionView);
                Button inventoryMission = inventoryGo.transform.Find("Mission Button").GetComponent<Button>();

                inventoryMission.GetComponentInChildren<Text>().text = mission;
                inventoryMission.onClick.AddListener(delegate { SetInventoryMission(mission, "DONE!", missions[mission][0].ToString()); });

                inventoryGo.transform.SetParent(inventoryMissionsScroll);


            }
            else
            {
                //---------- MAIN LIST MISSIONS ----------

                GameObject go = Instantiate(missionView);
                Button minigameMission = go.transform.Find("Mission Button").GetComponent<Button>();

                minigameMission.GetComponentInChildren<Text>().text = mission;
                go.transform.Find("Progression Text").GetComponent<Text>().text = (100 * missions[mission][0] / missions[mission][1]).ToString() + "%";
                minigameMission.onClick.AddListener(delegate { LoadMinigames(mission, missions[mission][0]); });

                go.transform.SetParent(missionsScroll);

                //---------- INVENTORY LIST MISSIONS ----------

                GameObject inventoryGo = Instantiate(inventoryMissionView);
                Button inventoryMission = inventoryGo.transform.Find("Mission Button").GetComponent<Button>();

                inventoryMission.GetComponentInChildren<Text>().text = mission;
                inventoryMission.onClick.AddListener(delegate { SetInventoryMission(mission, (100 * missions[mission][0] / missions[mission][1]).ToString() + "%", missions[mission][0].ToString()); });

                inventoryGo.transform.SetParent(inventoryMissionsScroll);

            }

        }
        string firstMission = missions.Keys.First();
        if (missions[firstMission] != null && missions[firstMission][0] >= missions[firstMission][1])
        {
            SetInventoryMission(firstMission, "DONE!", missions[firstMission][0].ToString());
        }
        else if (missions[firstMission] != null && missions[firstMission][0] < missions[firstMission][1]) 
        { 
            SetInventoryMission(firstMission, (100 * missions[firstMission][0] / missions[firstMission][1]).ToString() + "%", missions[firstMission][0].ToString()); 
        }


    }

    private void SetInventoryMission(string mission, string inventoryStatus, string inventoryNumber)
    {
        inventoryMissionTitle.text = mission;
        inventoryMissionStatus.text = inventoryStatus;
        inventoryCubeNumbers.text = inventoryNumber;

        if (inventoryStatus == "DONE!")
        {
            inventoryMissionStatus.color = Color.yellow;
            inventoryCubeNumbers.color = Color.yellow;
        }
        else
        {
            inventoryMissionStatus.color = Color.white;
            inventoryCubeNumbers.color = Color.white;
        }
    }

    private void GoToConstruct()
    {
        SceneManager.LoadScene("3D Constructor");
    }

    private void LoadMinigames(string mission, int inventory)
    {
        SaveCurrentMission(mission, inventory);
        SceneManager.LoadScene("Minigame Selection Screen");
    }

    

    public void ToLogin()
    {
        auth.SignOut();
        SceneManager.LoadScene("Auth Screen");
    }

    
}
