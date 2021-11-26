using System;
using System.Collections;
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
    private string username;

    public RectTransform missionsScroll;
    public GameObject missionView;
    public GameObject missionReadyView;

    public Canvas inventoryCanvas;
    public TextMeshProUGUI inventoryMissionTitle;
    public RectTransform inventoryMissionsScroll;
    public GameObject inventoryMissionView;
    public TextMeshProUGUI inventoryCubeNumbers;
    public TextMeshProUGUI inventoryMissionStatus;


    private DatabaseReference reference;

    public Font font;

    void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        LoadUser();
        LoadMissionsWithUser(FillMissionScroll);
        inventoryCanvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab)) inventoryCanvas.enabled = true;
        else inventoryCanvas.enabled = false;
    }

    private void FillMissionScroll(Dictionary<string, List<int>> missions)
    {

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
        //TODO: gestionar els nivells de cada minijoc i guardar els punts de partida on apareixeran les figures. Si una ja s'ha recollit, aleshores que no torni a apareixer
        //dins els minijocs associats a aquesta missio
        SaveCurrentMission(mission, inventory);
        SceneManager.LoadScene("Minigame Selection Screen");
    }

    private void LoadMissionsWithUser(Action<Dictionary<string, List<int>>> callbackFunction)
    {
        Dictionary<string, List<int>> missions = new Dictionary<string, List<int>>();

        var DBTask = reference.Child("Users").Child(username).Child("Missions").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("No missions");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot mission in snapshot.Children)
                {
                    List<int> progression = new List<int>();
                    progression.Add(int.Parse(mission.Child("inventory").Value.ToString()));
                    progression.Add(int.Parse(mission.Child("cubes").Value.ToString()));
                    missions[mission.Key.ToString()] = progression;
                    
                }

            }
            callbackFunction(missions);
        });
    }

    public void ToLogin()
    {
        SceneManager.LoadScene("Auth Screen");
    }

    private void LoadUser()
    {
        string path = Application.persistentDataPath + "/saveuser.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataUser data = JsonUtility.FromJson<SaveDataUser>(json);

            username = data.username;
        }
    }

    private void SaveCurrentMission(string mission, int inventory)
    {
        SaveDataCurrentMission data = new SaveDataCurrentMission();
        data.mission = mission;
        data.inventory = inventory;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savecurrentmission.json", json);
    }

    [System.Serializable]
    class SaveDataUser
    {
        public string username;

    }

    [System.Serializable]
    class SaveDataCurrentMission
    {
        public string mission;
        public int inventory;

    }
}
