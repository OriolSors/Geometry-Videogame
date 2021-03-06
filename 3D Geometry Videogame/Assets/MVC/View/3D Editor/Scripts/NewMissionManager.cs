using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class NewMissionManager : MonoBehaviour
{
    private List<Vector3> cubePositions;
    private Dictionary<string, List<string>> playersDict = new Dictionary<string, List<string>>();

    private List<string> players;
    public TMP_Dropdown selectPlayer;
    public GameObject labels;
    private Toggle[] toggles;
    public Button addNewPlayer;
    public Button createMission;
    private bool isDefaultMission = false;

    public TextMeshProUGUI chosenPlayer;

    public RectTransform playersPanel;
    public RectTransform settingsPanel;
    public RectTransform createPanel;

    public RectTransform playersScroll;
    public GameObject playerView;

    public Canvas confirmPlayerCanvas;
    private bool isAcceptedPlayer = false;
    public Canvas noPlayerCanvas;

    public Canvas setMissionNameCanvas;
    public Canvas noValidMissionNameCanvas;

    private MissionController missionController;

    private void Start()
    {
        missionController = new MissionController();

        toggles = labels.GetComponentsInChildren<Toggle>();

        confirmPlayerCanvas.enabled = false;
        noPlayerCanvas.enabled = false;
        setMissionNameCanvas.enabled = false;
        noValidMissionNameCanvas.enabled = false;

        createMission.interactable = false;

        LoadPlayers(SetPlayersToDropdown);
    }

    private void Update()
    {
        chosenPlayer.text = selectPlayer.options[selectPlayer.value].text;
    }

    public void PlayersAsLastSibling()
    {
        playersPanel.SetAsLastSibling();
    }

    public void SettingsAsLastSibling()
    {
        settingsPanel.SetAsLastSibling();
    }

    public void CreateAsLastSibling()
    {
        if (isAcceptedPlayer)
        {
            FillPlayersListChecking();
            createPanel.SetAsLastSibling();
            isAcceptedPlayer = false;
        }
    }

    public void FillPlayersListChecking()
    {
        foreach (Transform child in playersScroll.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string player in playersDict.Keys)
        {
            GameObject go = Instantiate(playerView);
            go.transform.Find("Username Text").GetComponent<TextMeshProUGUI>().text = player;
            go.transform.SetParent(playersScroll);
        }
    }


    public void SetUp(List<Vector3> cubePositions)
    {
        this.cubePositions = cubePositions;
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
        else if (characteristics.Count < 3)
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Pick 3 characteristics minimum.";
        }
        else if (!missionController.CheckValidCharacteristics(characteristics))
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Incompatible characteristics. Try another selection or add more.";
        }
        else if (player == "All players")
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Mission assigned to all players!";
            foreach (string player_aux in players)
            {
                if(!playersDict.ContainsKey(player_aux)) playersDict.Add(player_aux, characteristics);
            }
            isAcceptedPlayer = true;
        }

        else if (player == "Default mission")
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Mission assigned by default!";
            foreach (string player_aux in players)
            {
                if (!playersDict.ContainsKey(player_aux)) playersDict.Add(player_aux, characteristics);
            }
            isAcceptedPlayer = true;
            isDefaultMission = true;
        }

        else
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Mission assigned to " + player + "!";
            playersDict.Add(player, characteristics);
            isAcceptedPlayer = true;
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

    public void ConfirmNoValidMissionName()
    {
        noValidMissionNameCanvas.enabled = false;
        setMissionNameCanvas.enabled = true;
    }

    public void CreateMission()
    {
        if (playersDict.Count == 0)
        {
            noPlayerCanvas.enabled = true;
        }
        else
        {
            setMissionNameCanvas.enabled = true;

        }
        
    }

    public void SaveNewMission()
    {
        string missionName = setMissionNameCanvas.GetComponentInChildren<TMP_InputField>().text;
        if (!string.IsNullOrWhiteSpace(missionName))
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            missionName += " " + date;
            missionController.CreateNewMission(missionName, AuthController.Instance.GetCurrentUser(), cubePositions, isDefaultMission, playersDict);
            SceneManager.LoadScene("Designer Mission List Screen");
        }
        else
        {
            noValidMissionNameCanvas.enabled = true;
            setMissionNameCanvas.enabled = false;
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
        selectPlayer.AddOptions(this.players);
        if (players.Count != 0) createMission.interactable = true;
    }

}
