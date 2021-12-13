using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private float xRange = 5;

    public int scoreStreak = 0;

    public TextMeshProUGUI goodChallenge, neutralChallenge, badChallenge;
    public TextMeshProUGUI scoreStreakText;

    public Canvas figureObtainedCanvas;
    public Canvas scoreStreakCanvas;
    public Canvas gameOverCanvas;

    private CollectController collectController;
    private Dictionary<string, string> challenges;
    private Dictionary<string, int> figureScore;

    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        gameOverCanvas.enabled = false;
        figureObtainedCanvas.enabled = false;
        scoreStreakCanvas.enabled = false;

        collectController = new CollectController();

        challenges = collectController.ObtainChallenges(GetNamesOf(spawnManager.prefabs));
        SetChallengesToDisplay();

        figureScore = collectController.ObtainFigureScore();

    }

    private void SetChallengesToDisplay()
    {
        goodChallenge.text = challenges["good"];
        neutralChallenge.text = challenges["neutral"];
        badChallenge.text = challenges["bad"];
    }

    private List<string> GetNamesOf(GameObject[] prefabs)
    {
        List<string> prefabNames = new List<string>();
        foreach(GameObject prefab in prefabs)
        {
            prefabNames.Add(prefab.name);
        }

        return prefabNames;
    }
    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime);
        if (transform.position.x <= -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.z <= -4)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -4);
        }
        if (transform.position.z >= -2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -2);
        }

        scoreStreakText.text = scoreStreak.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Cube") && other.gameObject.GetComponent<Renderer>().material.name == "Gold (Instance)")
        {
            collectController.IncreaseInventory();
            figureObtainedCanvas.enabled = true;
            StartCoroutine(IndicatorFigureObtainedCoroutine());
            ResetStreak();
        }
        else
        {
            scoreStreak += figureScore[other.gameObject.tag];
            if (scoreStreak < 0) scoreStreak = 0;
            if (scoreStreak == 5)
            {
                spawnManager.StreakAchieved();
                scoreStreakCanvas.enabled = true;
                StartCoroutine(IndicatorScoreStreakCoroutine());

            }
            if (figureScore[other.gameObject.tag] < -500)
            {
                gameOverCanvas.enabled = true;
                StartCoroutine(IndicatorGameOverCoroutine());
                
            }
            
        }

        Destroy(other.gameObject);

    }

    public void ResetStreak()
    {
        scoreStreak = 0;
    }

    IEnumerator IndicatorGameOverCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        gameOverCanvas.enabled = false;
        ExitGame();
    }

    IEnumerator IndicatorFigureObtainedCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        figureObtainedCanvas.enabled = false;
    }

    IEnumerator IndicatorScoreStreakCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        scoreStreakCanvas.enabled = false;
    }

    public void ExitGame()
    {
        collectController.SaveCurrentMission();
        SceneManager.LoadScene("Minigame Selection Screen");
    }
    

}
