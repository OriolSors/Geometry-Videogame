using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections;
using System;

public class CanvasManager : MonoBehaviour
{

    public List<Vector3> cubePositions;

    public TextMeshProUGUI objectsLeft;
    private Color preColor;

    public Canvas labelsCanvas;
    public Canvas invalidGraphIndicator;

    private NewMissionManager missionManager;

    void Start()
    {
        missionManager = labelsCanvas.GetComponent<NewMissionManager>();

        labelsCanvas.enabled = false;
        invalidGraphIndicator.enabled = false;
        cubePositions = new List<Vector3>();
        cubePositions.Add(Vector3.zero);
        preColor = objectsLeft.color;

    }

    public void ResetConstruction()
    {
        SceneManager.LoadScene("3D Editor");
    }

    public void AddNewObject(Vector3 newPosition)
    {
        cubePositions.Add(Round(newPosition));
        objectsLeft.text = (int.Parse(objectsLeft.text) - 1).ToString();
        SetColor();
    }

    public void RemoveObject(Vector3 position)
    {
        Vector3 roundedPosition = Round(position);
        cubePositions.Remove(roundedPosition);
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

        if (IsValidGraph()) //TODO: check graph in progress to indicate invalid graph at real time
        {
            labelsCanvas.enabled = true;
            missionManager.SetUp(cubePositions);
            gameObject.SetActive(false);
        }
        else
        {
            invalidGraphIndicator.enabled = true;
            StartCoroutine(IndicatorInvalidGraphCoroutine());
        }
        
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Designer Mission List Screen");
    }

    private bool IsValidGraph()
    {
        Graph g = new Graph(cubePositions);
        return g.IsValidGraph();
    }

    IEnumerator IndicatorInvalidGraphCoroutine()
    {
        yield return new WaitForSeconds(0.85f);
        invalidGraphIndicator.enabled = false;
    }

    public static Vector3 Round(Vector3 vector3, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }

}
