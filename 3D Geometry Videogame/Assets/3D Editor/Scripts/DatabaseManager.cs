using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using Firebase.Extensions;
using Newtonsoft.Json;
using System.Linq;

public class DatabaseManager : MonoBehaviour
{
    private string designer;
    private int objectsNumber;
    private Dictionary<string, List<string>> playersDict = new Dictionary<string, List<string>>();

    public TMP_Dropdown selectPlayer;
    public GameObject labels;
    private Toggle[] toggles;
    public Button addNewPlayer;
    public Button createMission;

    public Canvas confirmPlayerCanvas;
    public Canvas noPlayerCanvas;

    private DatabaseReference reference;

    private void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

        toggles = labels.GetComponentsInChildren<Toggle>();

        confirmPlayerCanvas.enabled = false;
        noPlayerCanvas.enabled = false;

        createMission.interactable = false;

        LoadPlayers(SetPlayersToDropdown);
    }


    public void SetUp(string designer, int objectsNumber)
    {
        this.designer = designer;
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

        if (player == "All players")
        {
            //TODO: dinamicament tots els jugadors registrats tindran la missio
        }

        if (playersDict.ContainsKey(player))
        {
            confirmPlayerCanvas.enabled = true;
            confirmPlayerCanvas.GetComponentInChildren<TextMeshProUGUI>().text = player + " has already been assigned!";
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
            Mission mission = new Mission(designer, objectsNumber, playersDict);
            string json = JsonConvert.SerializeObject(mission);
            reference.Child("Missions").Child(date).SetRawJsonValueAsync(json);

            foreach (string player in playersDict.Keys)
            {
                PlayerMission playerMission = new PlayerMission(player, objectsNumber, 0, playersDict[player]);
                string json_player = JsonConvert.SerializeObject(playerMission);
                reference.Child("Users").Child(player).Child("Missions").Child(date).SetRawJsonValueAsync(json_player);
                reference.Child("Missions").Child(date).Child("playersDict").Child(player).Child("inventory").SetValueAsync(0);
            }
            SceneManager.LoadScene("Designer Mission List Screen");
        }
        
    }

    public void ExitScreen()
    {
        SceneManager.LoadScene("Designer Mission List Screen");
    }

    private void SetPlayersToDropdown(List<string> players)
    {
        selectPlayer.AddOptions(players);
        if (players.Count != 0) createMission.interactable = true;
    }

    private void LoadPlayers(Action<List<string>> callbackFunction)
    {
        List<string> players = new List<string>();
        var DBTask = reference.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("No players");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot s in snapshot.Children)
                {
                    if (s.Child("account").Value.ToString() == "Player") players.Add(s.Child("username").Value.ToString());
                }
            }

            callbackFunction(players);
        });
    }


    [System.Serializable]
    class Mission
    {
        public string designer;
        public int cubes;
        public Dictionary<string, List<string>> playersDict;

        public Mission(string designer, int cubes, Dictionary<string, List<string>> playersDict)
        {
            this.designer = designer;
            this.cubes = cubes;
            this.playersDict = playersDict;
        }
    }

    [System.Serializable]
    class PlayerMission
    {
        public string player;
        public int cubes;
        public Dictionary<string, Dictionary<int,bool>> waveCubeSpawn = new Dictionary<string, Dictionary<int, bool>>();
        public int inventory;
        public List<string> characteristics;

        public PlayerMission(string player, int cubes, int inventory, List<string> characteristics)
        {
            this.player = player;
            this.cubes = cubes;
            this.inventory = inventory;
            this.characteristics = characteristics;
            SetWaveNumberToSpawn();
        }

        public void SetWaveNumberToSpawn()
        {
            int quotient_down = Convert.ToInt32(Math.Floor((float)cubes / 2f)); //TODO: tenir en compte el joc Collect Game, aixi que s'haura de modificar aixo
            int[] waveSpawnArray = { quotient_down, quotient_down };
            
            for (int i = 0; i < waveSpawnArray.Length; i++)
            {
                if (waveSpawnArray.Sum() == cubes) break;
                waveSpawnArray[i]++;
            }

            var rand = new System.Random();
            

            Dictionary<int, bool> cubeWaveTatami = new Dictionary<int, bool>();
            Dictionary<int, bool> cubeWaveFootball = new Dictionary<int, bool>();

            foreach (int wavePos in Enumerable.Range(1, 5).OrderBy(x => rand.Next()).Take(waveSpawnArray[0]))
            {
                cubeWaveTatami[wavePos] = false;
            }

            foreach (int wavePos in Enumerable.Range(1, 5).OrderBy(x => rand.Next()).Take(waveSpawnArray[1]))
            {
                cubeWaveFootball[wavePos] = false;
            }

            waveCubeSpawn["tatami"] = cubeWaveTatami; 
            waveCubeSpawn["football"] = cubeWaveFootball;

        }
    }
}
