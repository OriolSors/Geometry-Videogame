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

    private DatabaseReference reference;

    private void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

        toggles = labels.GetComponentsInChildren<Toggle>();

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
        foreach (Toggle prop in toggles)
        {
            if (prop.isOn) characteristics.Add(prop.GetComponentInChildren<Text>().text);
            
        }
        playersDict.Add(selectPlayer.options[selectPlayer.value].text, characteristics);

    }

    public void CreateMission()
    {
        Mission mission = new Mission(designer, objectsNumber, playersDict);
        string json = JsonConvert.SerializeObject(mission);
        reference.Child("Missions").Child(DateTime.Now.ToString("yyyy-MM-dd\\THH:mm:ss")).SetRawJsonValueAsync(json);

        foreach (string player in playersDict.Keys)
        {
            PlayerMission playerMission = new PlayerMission(player, objectsNumber, 0, playersDict[player]);
            string json_player = JsonConvert.SerializeObject(playerMission);
            reference.Child("Users").Child(player).Child("Missions").Child(DateTime.Now.ToString("yyyy-MM-dd\\THH:mm:ss")).SetRawJsonValueAsync(json_player);
        }
        SceneManager.LoadScene("3D Editor");

    }

    private void SetPlayersToDropdown(List<string> players)
    {
        selectPlayer.AddOptions(players);
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
        public int inventory;
        public List<string> characteristics;

        public PlayerMission(string player, int cubes, int inventory, List<string> characteristics)
        {
            this.player = player;
            this.cubes = cubes;
            this.inventory = inventory;
            this.characteristics = characteristics;
        }
    }
}
