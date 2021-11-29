using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using Firebase.Extensions;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class NewAccountScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField userName;

    [SerializeField]
    private TMP_Dropdown accountTypeOption;

    private List<PlayerMission> defaultMissions;

    private DatabaseReference reference;

    private SceneController sceneController;

    private void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        sceneController = GameObject.Find("Scene Controller").GetComponent<SceneController>();
    }

    public void CreateNewUser()
    {

        string name = userName.text;
        string account = accountTypeOption.options[accountTypeOption.value].text;
        IsNewUser(name, account, AssignUser);

    }

    private void AssignUser(bool available, string name, string account)
    {
        if (available)
        {
            User newUser = new User(name, account);
            string json = JsonUtility.ToJson(newUser);
            reference.Child("Users").Child(name).SetRawJsonValueAsync(json);
            sceneController.LoadCorrectScene(name, null);
        }
        else
        {
            userName.placeholder.GetComponent<TextMeshProUGUI>().text = "User exists";
            userName.placeholder.color = Color.red;
            userName.text = "";

        }
    }

    private void IsNewUser(string userName, string accountTypeOption, Action<bool,string,string> callbackFunction)
    {
        bool isNewUser = true;

        var DBTask = reference.Child("Users").Child(userName).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("Full new user");
                
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child("username").Value.ToString() == userName)
                {
                    isNewUser = false;

                }
            }
            callbackFunction(isNewUser, userName, accountTypeOption);
        });

    }

    public class User
    {
        public string username;
        public string account;

        public User(string username, string account)
        {
            this.username = username;
            this.account = account;
        }
    }

    [System.Serializable]
    class PlayerMission
    {
        public string player;
        public int cubes;
        public Dictionary<string, Dictionary<int, bool>> waveCubeSpawn = new Dictionary<string, Dictionary<int, bool>>();
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
