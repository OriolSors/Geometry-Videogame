using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallTatamiController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private bool stopped = false;

    public bool hasPowerup;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;
    public Canvas bonusCanvas;
    public Canvas noBonusCanvas;

    public bool gameOver = false;
    public Canvas gameOverCanvas;

    private List<string> characteristics;
    private List<Question> questions;
    public Canvas questionCanvas;
    public TextMeshProUGUI questionText;
    public Button firstAnswer;
    public Button secondAnswer;
    public Button thirdAnswer;
    public Button fourthAnswer;


    private string username, mission;
    private int inventory;

    private SpawnTatamiManager spawnManagerScript;
    private DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        bonusCanvas.enabled = false;
        noBonusCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        questionCanvas.enabled = false;

        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnTatamiManager>();

        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

        LoadUser();
        LoadCurrentMission();
        LoadCharacteristics();
        LoadQuestions(SetQuestions);
    }

    private void SetQuestions(List<Question> questions)
    {
        this.questions = questions;
    }

    private void LoadQuestions(Action<List<Question>> callbackFunction)
    {
        List<Question> questions = new List<Question>();
        var DBTask = reference.Child("Questions").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("No questions");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot characteristic in snapshot.Children)
                {
                    if (characteristics.Contains(characteristic.Key.ToString()))
                    {
                        foreach (DataSnapshot question in characteristic.Children)
                        {
                            string q = question.Children.First().Key.ToString();
                            string correct;

                            string first = question.Children.First().Children.ElementAt(0).Key.ToString();
                            string second = question.Children.First().Children.ElementAt(1).Key.ToString();
                            string third = question.Children.First().Children.ElementAt(2).Key.ToString();
                            string fourth = question.Children.First().Children.ElementAt(3).Key.ToString();

                            if ((bool)question.Children.First().Children.ElementAt(0).Value) correct = first;
                            else if ((bool)question.Children.First().Children.ElementAt(1).Value) correct = second;
                            else if ((bool)question.Children.First().Children.ElementAt(2).Value) correct = third;
                            else correct = fourth;

                            questions.Add(new Question(q, first, second, third, fourth, correct));
                        }
                        
                    }
                }

                callbackFunction(questions);
                
            }

            
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
            gameOver = true;
            gameOverCanvas.enabled = true;
            StartCoroutine(IndicatorGameOverCoroutine());

        }
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        float forwardInput = Input.GetAxis("Vertical");
        if (!stopped) playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Powerup")) 
        {
            Destroy(other.gameObject);

            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            stopped = true;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().StopMovement();
            }

            questionCanvas.enabled = true;
            SetCurrentQuestion();
            

        }
        else if (other.CompareTag("Cube"))
        {
            inventory++;
            spawnManagerScript.SetCollectedCube();
            Destroy(other.gameObject);
        }
    }

    private void SetCurrentQuestion()
    {
        var rand = new System.Random();
        int index = rand.Next(questions.Count);
        Question currentQuestion = questions[index];
        questionText.text = currentQuestion.question;
        firstAnswer.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.first;
        secondAnswer.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.second;
        thirdAnswer.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.third;
        fourthAnswer.GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.fourth;

        switch (currentQuestion.CorrectIndexAnswer())
        {
            case "first":
                firstAnswer.onClick.AddListener(delegate { GetBonus("first"); });
                secondAnswer.onClick.AddListener(delegate { LoseBonus("first"); });
                thirdAnswer.onClick.AddListener(delegate { LoseBonus("first"); });
                fourthAnswer.onClick.AddListener(delegate { LoseBonus("first"); });

                break;
                
            case "second":
                firstAnswer.onClick.AddListener(delegate { LoseBonus("second"); });
                secondAnswer.onClick.AddListener(delegate { GetBonus("second"); });
                thirdAnswer.onClick.AddListener(delegate { LoseBonus("second"); });
                fourthAnswer.onClick.AddListener(delegate { LoseBonus("second"); });
                break;

            case "third":
                firstAnswer.onClick.AddListener(delegate { LoseBonus("third"); });
                secondAnswer.onClick.AddListener(delegate { LoseBonus("third"); });
                thirdAnswer.onClick.AddListener(delegate { GetBonus("third"); });
                fourthAnswer.onClick.AddListener(delegate { LoseBonus("third"); });
                break;

            case "fourth":
                firstAnswer.onClick.AddListener(delegate { LoseBonus("fourth"); });
                secondAnswer.onClick.AddListener(delegate { LoseBonus("fourth"); });
                thirdAnswer.onClick.AddListener(delegate { LoseBonus("fourth"); });
                fourthAnswer.onClick.AddListener(delegate { GetBonus("fourth"); });
                break;
        }
        

    }

    private void RemoveListeners(string correct)
    {

        firstAnswer.onClick.RemoveAllListeners();
        secondAnswer.onClick.RemoveAllListeners();
        thirdAnswer.onClick.RemoveAllListeners();
        fourthAnswer.onClick.RemoveAllListeners();

        /*
        switch (correct)
        {
            case "first":
                firstAnswer.onClick.RemoveListener(delegate { GetBonus("first"); });
                secondAnswer.onClick.RemoveListener(delegate { LoseBonus("first"); });
                thirdAnswer.onClick.RemoveListener(delegate { LoseBonus("first"); });
                fourthAnswer.onClick.RemoveListener(delegate { LoseBonus("first"); });

                break;

            case "second":
                firstAnswer.onClick.RemoveListener(delegate { LoseBonus("second"); });
                secondAnswer.onClick.RemoveListener(delegate { GetBonus("second"); });
                thirdAnswer.onClick.RemoveListener(delegate { LoseBonus("second"); });
                fourthAnswer.onClick.RemoveListener(delegate { LoseBonus("second"); });
                break;

            case "third":
                firstAnswer.onClick.RemoveListener(delegate { LoseBonus("third"); });
                secondAnswer.onClick.RemoveListener(delegate { LoseBonus("third"); });
                thirdAnswer.onClick.RemoveListener(delegate { GetBonus("third"); });
                fourthAnswer.onClick.RemoveListener(delegate { LoseBonus("third"); });
                break;

            case "fourth":
                firstAnswer.onClick.RemoveListener(delegate { LoseBonus("fourth"); });
                secondAnswer.onClick.RemoveListener(delegate { LoseBonus("fourth"); });
                thirdAnswer.onClick.RemoveListener(delegate { LoseBonus("fourth"); });
                fourthAnswer.onClick.RemoveListener(delegate { GetBonus("fourth"); });
                break;
        }

        */
    }

    private void LoseBonus(string correct)
    {
        RemoveListeners(correct);
        stopped = false;
        questionCanvas.enabled = false;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().RestartMovement();
        }
        noBonusCanvas.enabled = true;
        StartCoroutine(IndicatorNoBonusCoroutine());

    }

    private void GetBonus(string correct)
    {
        stopped = false;
        RemoveListeners(correct);
        questionCanvas.enabled = false;
        bonusCanvas.enabled = true;
        StartCoroutine(IndicatorBonusCoroutine());

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }

    }

    IEnumerator IndicatorGameOverCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        gameOverCanvas.enabled = false;
        SaveCurrentMission();
        SceneManager.LoadScene("Minigame Selection Screen");
    }

    IEnumerator IndicatorBonusCoroutine()
    {
        StartCoroutine(PowerupCountdownRoutine());
        yield return new WaitForSeconds(1.5f);
        hasPowerup = true;
        powerupIndicator.gameObject.SetActive(true);
        bonusCanvas.enabled = false;
    }

    IEnumerator IndicatorNoBonusCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        noBonusCanvas.enabled = false;
    }

    IEnumerator PowerupCountdownRoutine() {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().StopMovement();
        }

        yield return new WaitForSeconds(7);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().RestartMovement();
        }
        powerupIndicator.gameObject.SetActive(false);
        hasPowerup = false;
    }

    public void ExitGame()
    {
        SaveCurrentMission();
        SceneManager.LoadScene("Minigame Selection Screen");
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

    private void LoadCharacteristics()
    {
        string path = Application.persistentDataPath + "/savecharacteristics.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataCharacteristics data = JsonUtility.FromJson<SaveDataCharacteristics>(json);

            characteristics = data.characteristics;
        }
    }

    private void LoadCurrentMission()
    {
        string path = Application.persistentDataPath + "/savecurrentmission.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataCurrentMission data = JsonUtility.FromJson<SaveDataCurrentMission>(json);

            mission = data.mission;
            inventory = data.inventory;
        }
    }

    private void SaveCurrentMission()
    {
        SaveDataCurrentMission data = new SaveDataCurrentMission();
        data.mission = mission;
        data.inventory = inventory;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savecurrentmission.json", json);
        reference.Child("Users").Child(username).Child("Missions").Child(mission).Child("inventory").SetValueAsync(inventory);
        reference.Child("Missions").Child(mission).Child("playersDict").Child(username).Child("inventory").SetValueAsync(inventory);
        reference.Child("Users").Child(username).Child("Missions").Child(mission).Child("waveCubeSpawn").Child("tatami").SetValueAsync(spawnManagerScript.GetWaveCubes());
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

    [System.Serializable]
    class SaveDataCharacteristics
    {
        public List<string> characteristics;
    }

    public class Question
    {
        public string question, first, second, third, fourth, correct;

        public Question (string question, string first, string second, string third, string fourth, string correct)
        {
            this.question = question;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.correct = correct;
        }

        public string CorrectIndexAnswer()
        {
            if (first == correct) return "first";
            else if (second == correct) return "second";
            else if (third == correct) return "third";
            else return "fourth";
        }
    }
}
