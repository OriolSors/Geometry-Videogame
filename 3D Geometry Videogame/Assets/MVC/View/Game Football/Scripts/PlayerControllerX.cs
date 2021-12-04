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

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;
    private bool stopped = false;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 10;
    public Canvas bonusCanvas;
    public Canvas noBonusCanvas;
    public Canvas figureObtainedCanvas;

    private List<string> characteristics;
    private List<Question> questions;
    public Canvas questionCanvas;
    public TextMeshProUGUI questionText;
    public Button firstAnswer;
    public Button secondAnswer;
    public Button thirdAnswer;
    public Button fourthAnswer;


    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    public ParticleSystem dirtParticle;

    private string username, mission;
    private int inventory;

    private SpawnManagerX spawnManagerScript;
    private DatabaseReference reference;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        bonusCanvas.enabled = false;
        noBonusCanvas.enabled = false;
        questionCanvas.enabled = false;
        figureObtainedCanvas.enabled = false;

        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerX>();

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

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dirtParticle.gameObject.transform.position = transform.position;
            dirtParticle.Play();
            playerRb.AddForce(focalPoint.transform.forward * speed * 2 * Time.deltaTime, ForceMode.Impulse);
        }
        
        
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        if (!stopped) playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);

            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            stopped = true;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemy.GetComponent<EnemyX>().StopMovement();
            }

            questionCanvas.enabled = true;
            SetCurrentQuestion();

        }
        else if (other.CompareTag("Cube"))
        {
            inventory++;
            spawnManagerScript.SetCollectedCube();
            Destroy(other.gameObject);
            figureObtainedCanvas.enabled = true;
            StartCoroutine(IndicatorFigureObtainedCoroutine());
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

        firstAnswer.onClick.AddListener(delegate { CheckBonus(currentQuestion, currentQuestion.first); });
        secondAnswer.onClick.AddListener(delegate { CheckBonus(currentQuestion, currentQuestion.second); });
        thirdAnswer.onClick.AddListener(delegate { CheckBonus(currentQuestion, currentQuestion.third); });
        fourthAnswer.onClick.AddListener(delegate { CheckBonus(currentQuestion, currentQuestion.fourth); });

    }

    private void CheckBonus(Question question, string textAnswer)
    {
        if (question.CheckCorrectAnswer(textAnswer)) GetBonus();
        else LoseBonus();
    }

    private void RemoveListeners()
    {

        firstAnswer.onClick.RemoveAllListeners();
        secondAnswer.onClick.RemoveAllListeners();
        thirdAnswer.onClick.RemoveAllListeners();
        fourthAnswer.onClick.RemoveAllListeners();
    }

    private void LoseBonus()
    {
        RemoveListeners();
        stopped = false;
        questionCanvas.enabled = false;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<EnemyX>().RestartMovement();
        }
        noBonusCanvas.enabled = true;
        StartCoroutine(IndicatorNoBonusCoroutine());

    }

    private void GetBonus()
    {
        stopped = false;
        RemoveListeners();
        questionCanvas.enabled = false;
        bonusCanvas.enabled = true;
        StartCoroutine(IndicatorBonusCoroutine());

    }


    IEnumerator IndicatorBonusCoroutine()
    {
        StartCoroutine(PowerupCooldown());
        yield return new WaitForSeconds(1.5f);
        hasPowerup = true;
        powerupIndicator.gameObject.SetActive(true);
        bonusCanvas.enabled = false;
        
    }

    IEnumerator IndicatorFigureObtainedCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        figureObtainedCanvas.enabled = false;
    }

    IEnumerator IndicatorNoBonusCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        noBonusCanvas.enabled = false;
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<EnemyX>().StopMovement();
        }

        yield return new WaitForSeconds(powerUpDuration);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<EnemyX>().RestartMovement();
        }

        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer =  other.gameObject.transform.position - transform.position; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
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

    private void SaveCurrentMission()
    {
        SaveDataCurrentMission data = new SaveDataCurrentMission();
        data.mission = mission;
        data.inventory = inventory;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savecurrentmission.json", json);
        reference.Child("Users").Child(username).Child("Missions").Child(mission).Child("inventory").SetValueAsync(inventory);
        reference.Child("Missions").Child(mission).Child("playersDict").Child(username).Child("inventory").SetValueAsync(inventory);
        reference.Child("Users").Child(username).Child("Missions").Child(mission).Child("waveCubeSpawn").Child("football").SetValueAsync(spawnManagerScript.GetWaveCubes());
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

        public Question(string question, string first, string second, string third, string fourth, string correct)
        {
            this.question = question;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.correct = correct;
        }

        public bool CheckCorrectAnswer(string text)
        {
            return correct == text;
        }
    }

}
