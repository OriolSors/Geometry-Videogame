using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private float xRange = 5;

    private string username, mission;
    private int inventory;

    private DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        LoadUser();
        LoadCurrentMission();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: Diferenciar figures bones, dolentes o neutres segons l'enunciat. Implementar bonus.
        
        if (other.CompareTag("Cube") && other.gameObject.GetComponent<Renderer>().material.name == "Gold (Instance)")
        {
            inventory++;
        }
        Destroy(other.gameObject);

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
