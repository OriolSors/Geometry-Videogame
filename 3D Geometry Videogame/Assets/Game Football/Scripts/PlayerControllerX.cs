using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 10;
    public Canvas bonusCanvas;

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

        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerX>();

        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        LoadUser();
        LoadCurrentMission();
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
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            //TODO: activar Canvas amb una pregunta de la tematica
            bonusCanvas.enabled = true;
            StartCoroutine(IndicatorBonusCoroutine());

        }
        else if (other.CompareTag("Cube"))
        {
            inventory++;
            spawnManagerScript.SetCollectedCube();
            Destroy(other.gameObject);
        }

    }

    IEnumerator IndicatorBonusCoroutine()
    {
        StartCoroutine(PowerupCooldown());
        yield return new WaitForSeconds(1.5f);
        hasPowerup = true;
        powerupIndicator.gameObject.SetActive(true);
        bonusCanvas.enabled = false;
        
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

    private void SaveCurrentMission()
    {
        SaveDataCurrentMission data = new SaveDataCurrentMission();
        data.mission = mission;
        data.inventory = inventory;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savecurrentmission.json", json);
        reference.Child("Users").Child(username).Child("Missions").Child(mission).Child("inventory").SetValueAsync(inventory);
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


}
