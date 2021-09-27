using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    public GameObject prefabInst;
    public Material goldMaterial;
    private float dynamicEdge;

    private bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCube()
    {
        StartCoroutine(LoadData());
        
    }

    private IEnumerator LoadData()
    {
        DatabaseReference reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;

        var DBTask = reference.Child("Bag").Child("Cube").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.Log("fail");
        }
        else if (DBTask.Result.Value == null)
        {
            dynamicEdge = 4f;
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            dynamicEdge = float.Parse(snapshot.Child("edge").Value.ToString());
            collected = bool.Parse(snapshot.Child("collected").Value.ToString());

            GameObject goldInst = Instantiate(prefabInst, new Vector3(0, 0, 0), prefabInst.transform.rotation);
            goldInst.GetComponent<Renderer>().material = goldMaterial;
            goldInst.transform.localScale = new Vector3(dynamicEdge, dynamicEdge, dynamicEdge);
            goldInst.GetComponent<Rigidbody>().useGravity = false;
        }


    }

}
