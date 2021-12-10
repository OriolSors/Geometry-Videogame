using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class NewMissionManager : MonoBehaviour
{
    private int objectsNumber;
    private Dictionary<string, List<string>> playersDict = new Dictionary<string, List<string>>();

    private List<string> players;
    public TMP_Dropdown selectPlayer;
    public GameObject labels;
    private Toggle[] toggles;
    public Button addNewPlayer;
    public Button createMission;
    private bool isDefaultMission = false;

    public Canvas confirmPlayerCanvas;
    public Canvas noPlayerCanvas;

    private MissionController missionController;

    private void Start()
    {
        missionController = new MissionController();

        toggles = labels.GetComponentsInChildren<Toggle>();

        confirmPlayerCanvas.enabled = false;
        noPlayerCanvas.enabled = false;

        createMission.interactable = false;

        LoadPlayers(SetPlayersToDropdown);
    }


    public void SetUp(int objectsNumber)
    {
        this.objectsNumber = objectsNumber;
    }

    public void AddNewPlayer()
    {
        List<string> characteristics = new List<string>();
        string player = selectPlayer.options[selectPlayer.value].text;
        foreach (Toggle prop in toggles)
        {
            if (prop.isOn) characteristics.Add(prop.GetComponentInChildren<TextMeshProUGUI>().text);
            
        }

        if (playersDict.ContainsKey(player))
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = player + " has already been assigned!";
        }
        else if (player == "All players")
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Mission assigned to all players!";
            foreach (string player_aux in players)
            {
                playersDict.Add(player_aux, characteristics);
            }
        }

        else if (player == "Default mission")
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Mission assigned by default!";
            foreach (string player_aux in players)
            {
                playersDict.Add(player_aux, characteristics);
            }
            isDefaultMission = true;
        }

        else
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Mission assigned to " + player + "!";
            playersDict.Add(player, characteristics);
        }
        
        

    }

    public void ConfirmPlayer()
    {
        confirmPlayerCanvas.enabled = false;
    }

    public void ConfirmNoPlayer()
    {
        noPlayerCanvas.enabled = false;
    }

    public void CreateMission()
    {
        if (playersDict.Count == 0)
        {
            noPlayerCanvas.enabled = true;
        }
        else
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd\\THH:mm:ss");
            missionController.CreateNewMission(date, AuthController.Instance.GetCurrentUser(), objectsNumber, isDefaultMission, playersDict);
            SceneManager.LoadScene("Designer Mission List Screen");
        }
        
    }

    public void ExitScreen()
    {
        SceneManager.LoadScene("Designer Mission List Screen");
    }

    private void LoadPlayers(Action<List<string>> SetPlayersToDropdown)
    {
        missionController.GetAllPlayerNames(SetPlayersToDropdown);
    }

    private void SetPlayersToDropdown(List<string> players)
    {
        this.players = players;
        selectPlayer.AddOptions(players);
        if (players.Count != 0) createMission.interactable = true;
    }

}
