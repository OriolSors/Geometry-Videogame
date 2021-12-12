using System.Collections;
using System.Collections.Generic;
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

    private SpawnManagerX spawnManagerScript;
    private FootballController footballController;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        bonusCanvas.enabled = false;
        noBonusCanvas.enabled = false;
        questionCanvas.enabled = false;
        figureObtainedCanvas.enabled = false;

        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerX>();

        footballController = new FootballController();
        spawnManagerScript.SetWavesDict(footballController.GetWavesDict());
        LoadQuestions();
        
    }

    private async void LoadQuestions()
    {
        await footballController.LoadQuestions();
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
            footballController.IncreaseInventory();
            spawnManagerScript.SetCollectedCube();
            Destroy(other.gameObject);
            figureObtainedCanvas.enabled = true;
            StartCoroutine(IndicatorFigureObtainedCoroutine());
        }

    }

    private void SetCurrentQuestion()
    {
        Question currentQuestion = footballController.RequestQuestion();
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
        footballController.UpdateMissionPlayer(spawnManagerScript.GetWaveCubes());
        SceneManager.LoadScene("Minigame Selection Screen");
    }

}
