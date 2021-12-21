using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class CanvasManager : MonoBehaviour
{

    public List<Vector3> cubePositions;
    public TextMeshProUGUI objectsLeft;
    private Color preColor;

    public Canvas labelsCanvas;

    private NewMissionManager missionManager;

    void Start()
    {
        missionManager = labelsCanvas.GetComponent<NewMissionManager>();
        labelsCanvas.enabled = false;

        cubePositions = new List<Vector3>();
        cubePositions.Add(Vector3.zero);
        preColor = objectsLeft.color;

    }

    public void ResetConstruction()
    {
        SceneManager.LoadScene("3D Editor");
    }

    public void AddNewObject(Vector3 position)
    {

        cubePositions.Add(position);
        objectsLeft.text = (int.Parse(objectsLeft.text) - 1).ToString();
        SetColor();
    }

    public void RemoveObject(Vector3 position)
    {
        cubePositions.Remove(position);
        objectsLeft.text = (int.Parse(objectsLeft.text) + 1).ToString();
        SetColor();
    }

    public bool ObjectsAvailables()
    {
        return int.Parse(objectsLeft.text) > 0;
    }

    private void SetColor()
    {
        if (int.Parse(objectsLeft.text) == 0)
        {
            objectsLeft.color = Color.red;
        }
        else
        {
            objectsLeft.color = preColor;
        }
    }


    public void SaveConstruction()
    {
        labelsCanvas.enabled = true;
        missionManager.SetUp(cubePositions);
        /*
        Mission mission = new Mission(username, cubePositions.Count);
        string json = JsonUtility.ToJson(mission);
        reference.Child("Users").Child(username).Child("Missions").Child(DateTime.Now.ToString()).SetRawJsonValueAsync(json); //TODO: Change to Push
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        */
    }

}
