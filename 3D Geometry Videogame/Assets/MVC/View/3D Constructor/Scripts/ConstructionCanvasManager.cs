using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections;
using System;

public class ConstructionCanvasManager : MonoBehaviour
{

    public List<Vector3> cubePositions;
    public TextMeshProUGUI objectsLeft;
    private Color preColor;

    public Canvas constructionCorrectCanvas;
    public Canvas constructionIncorrectCanvas;

    private ConstructionController constructionController;

    public GameObject matricesPanel;
    private ConstructionGridManager constructionGridManager;

    void Start()
    {

        cubePositions = new List<Vector3>();
        cubePositions.Add(Vector3.zero);
        preColor = objectsLeft.color;

        constructionCorrectCanvas.enabled = false;
        constructionIncorrectCanvas.enabled = false;

        constructionController = ConstructionController.Instance;
        constructionController.SetUpValues();
        constructionController.GetTargetTileMatrices();
        constructionController.GetUserTileMatrices(cubePositions);
        objectsLeft.text = (constructionController.GetNumberOfCubes()-1).ToString();

        constructionGridManager = matricesPanel.GetComponent<ConstructionGridManager>();
        constructionGridManager.InitializeValues();
        constructionGridManager.SpawnTiles();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Tab)) matricesPanel.SetActive(true);
        else matricesPanel.SetActive(false);

    }

    public void ResetConstruction()
    {
        SceneManager.LoadScene("3D Constructor");
    }

    public void ExitConstruction()
    {
        SceneManager.LoadScene("Player Mission List Screen");
    }

    private void UpdateUserTiles()
    {
        constructionController.GetUserTileMatrices(cubePositions);
        constructionGridManager.UpdateUserTiles();

    }

    public void AddNewObject(Vector3 position)
    {

        cubePositions.Add(Round(position));
        UpdateUserTiles();
        objectsLeft.text = (int.Parse(objectsLeft.text) - 1).ToString();
        SetColor();
    }


    public void RemoveObject(Vector3 position)
    {
        cubePositions.Remove(Round(position));
        UpdateUserTiles();
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


    public void CheckConstruction()
    {

        if (constructionController.CheckCubePositions(cubePositions))
        {
            constructionCorrectCanvas.enabled = true;
            StartCoroutine(IndicatorCorrectConstructionCoroutine());
        }
        else
        {
            constructionIncorrectCanvas.enabled = true;
            StartCoroutine(IndicatorIncorrectConstructionCoroutine());
        }
        

    }

    IEnumerator IndicatorCorrectConstructionCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        constructionCorrectCanvas.enabled = false;
        SceneManager.LoadScene("Player Mission List Screen");

    }

    IEnumerator IndicatorIncorrectConstructionCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        constructionIncorrectCanvas.enabled = false;
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
