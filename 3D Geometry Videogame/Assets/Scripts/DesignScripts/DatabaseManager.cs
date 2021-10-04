using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
    /*
    public Slider scaleSlider;
    private float edge;
    public GameObject cubeModel;
    // Start is called before the first frame update
    void Start()
    {
        scaleSlider.minValue = 0.5f;
        scaleSlider.maxValue = 1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        edge = scaleSlider.value;
        cubeModel.transform.localScale = new Vector3(edge, edge, edge);

        
    }

    private void FixedUpdate()
    {
        cubeModel.transform.Rotate(Vector3.forward * 15 * Time.fixedDeltaTime);
        cubeModel.transform.Rotate(Vector3.up * 15 * Time.fixedDeltaTime);
        cubeModel.transform.Rotate(Vector3.right * 15 * Time.fixedDeltaTime);
    }

    public void SaveData()
    {
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        Cube cube = new Cube(edge);
        string json = JsonUtility.ToJson(cube);
        reference.Child("Figures").Child("Cube").SetRawJsonValueAsync(json);

        SceneManager.LoadScene(1);
    }

    */
}
