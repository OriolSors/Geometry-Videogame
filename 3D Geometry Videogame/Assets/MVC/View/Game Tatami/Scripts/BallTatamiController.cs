using System.Collections;
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
    public Canvas figureObtainedCanvas;

    public bool gameOver = false;
    public Canvas gameOverCanvas;

    public Canvas questionCanvas;
    public TextMeshProUGUI questionText;
    public Button firstAnswer;
    public Button secondAnswer;
    public Button thirdAnswer;
    public Button fourthAnswer;

    private SpawnTatamiManager spawnManagerScript;
    private TatamiController tatamiController;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        bonusCanvas.enabled = false;
        noBonusCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        questionCanvas.enabled = false;
        figureObtainedCanvas.enabled = false;

        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnTatamiManager>();
        tatamiController = new TatamiController();
        spawnManagerScript.SetWavesDict(tatamiController.GetWavesDict());

        LoadQuestions();
    }

    private async void LoadQuestions()
    {
        await tatamiController.LoadQuestions();
    }

    // Update is called once per frame
    void FixedUpdate()
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
            tatamiController.IncreaseInventory();
            spawnManagerScript.SetCollectedCube();
            Destroy(other.gameObject);
            figureObtainedCanvas.enabled = true;
            StartCoroutine(IndicatorFigureObtainedCoroutine());
        }
    }

    private void SetCurrentQuestion()
    {
        
        Question currentQuestion = tatamiController.RequestQuestion();
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
            enemy.GetComponent<Enemy>().RestartMovement();
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

    IEnumerator IndicatorFigureObtainedCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        figureObtainedCanvas.enabled = false;
    }

    IEnumerator IndicatorGameOverCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        gameOverCanvas.enabled = false;
        tatamiController.UpdateMissionPlayer(spawnManagerScript.GetWaveCubes());
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
        tatamiController.UpdateMissionPlayer(spawnManagerScript.GetWaveCubes());
        SceneManager.LoadScene("Minigame Selection Screen");
    }

}
