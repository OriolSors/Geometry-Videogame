using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallTatamiController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;

    public bool hasPowerup;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;

    public bool gameOver = false;

    private string username, mission;
    private int inventory;

    private DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        LoadUser();
        LoadCurrentMission();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
            gameOver = true;
            SaveCurrentMission();
            SceneManager.LoadScene("Minigame Selection Screen");

        }
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: checkear i activar alguna pregunta associada amb les caracteristiques per poder agafar el bonus. Diferenciar bonus de orientacio cap a la figura
        //i de xoc amb les boles enemigues 
        if (other.CompareTag("Powerup")) 
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
        else if (other.CompareTag("Cube"))
        {
            inventory++;
            Destroy(other.gameObject);
        }
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
