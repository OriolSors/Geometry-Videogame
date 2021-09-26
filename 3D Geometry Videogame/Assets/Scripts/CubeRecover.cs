using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Globalization;

public class CubeRecover : MonoBehaviour
{

    public GameObject prefabCube;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadData());
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LoadData()
    {
        DatabaseReference reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

        var DBTask = reference.Child("Figures").Child("Cube").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.Log("fail");
        }
        else if (DBTask.Result.Value == null)
        {
            Instantiate(prefabCube, new Vector3(0, 10, 0), Quaternion.identity);
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            float edge = float.Parse(snapshot.Child("edge").Value.ToString());
            prefabCube.transform.localScale = new Vector3(edge, edge, edge);
            Instantiate(prefabCube, new Vector3(0, 10, 0), Quaternion.identity);
        }

        
    }
}
