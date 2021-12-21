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

    public void CreateMission()
    {
        if (playersDict.Count == 0)
        {
            noPlayerCanvas.enabled = true;
        }
        else
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd\\THH:mm:ss");

            List<Vector3> roundedCubePositions = new List<Vector3>();

            foreach(Vector3 cubePosition in cubePositions)
            {
                roundedCubePositions.Add(Round(cubePosition, 2));
            }
            
            missionController.CreateNewMission(date, AuthController.Instance.GetCurrentUser(), roundedCubePositions, isDefaultMission, playersDict);
            SceneManager.LoadScene("Designer Mission List Screen");
        }
        
    }

    public static Vector3 Round(Vector3 vector3, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
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
