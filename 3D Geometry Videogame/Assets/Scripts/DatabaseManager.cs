using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class DatabaseManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData()
    {
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
        Cube cube = new Cube();
        string json = JsonUtility.ToJson(cube);
        reference.Child("Figures").Child("Cube").SetRawJsonValueAsync(json);

        SceneManager.LoadScene(1);
    }
}
