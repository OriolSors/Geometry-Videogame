using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 boundaryRight, boundaryLeft, boundaryForward, boundaryBack;
    private float xRange = 5;

    private SpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
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
        
        if (other.CompareTag("Cube") && other.gameObject.GetComponent<Renderer>().material.name == "Gold (Instance)")
        {
            PlayerInventory bag = new PlayerInventory(new Cube(spawnManager.dynamicEdge), true);
            DatabaseReference reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
            string json = JsonUtility.ToJson(bag);
            reference.Child("Bag").Child("Cube").SetRawJsonValueAsync(json);
        }
        Destroy(other.gameObject);
    }

    public void ToConstruct()
    {
        SceneManager.LoadScene(2);
    }

}
