using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections;
using System;
using System.Linq;

public class ConstructionCanvasManager : MonoBehaviour
{

    public TextMeshProUGUI objectsLeft;
    private Color preColor;

    public Canvas constructionCorrectCanvas;
    public Canvas constructionIncorrectCanvas;
    public Canvas invalidPositionIndicator;

    public GameObject matricesPanel;
    private ConstructionGridManager constructionGridManager;

    private ConstructionBoundaryBoxController boundaryBoxController;

    void Start()
    {

        preColor = objectsLeft.color;

        constructionCorrectCanvas.enabled = false;
        constructionIncorrectCanvas.enabled = false;
        invalidPositionIndicator.enabled = false;

        constructionGridManager = matricesPanel.GetComponent<ConstructionGridManager>();
        constructionGridManager.InitializeValues();
        constructionGridManager.SpawnTiles();

        boundaryBoxController = GameObject.Find("Boundary Box").GetComponent<ConstructionBoundaryBoxController>();
        objectsLeft.text = boundaryBoxController.GetCubesLeft().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.Tab)) matricesPanel.SetActive(true);
        else matricesPanel.SetActive(false);
        */
        
    }

    public void ResetConstruction()
    {
        SceneManager.LoadScene("3D Constructor");
    }

    public void ExitConstruction()
    {
        SceneManager.LoadScene("Player Mission List Screen");
    }



    public void AddNewObject()
    {
        constructionGridManager.UpdateUserTiles();
        objectsLeft.text = boundaryBoxController.GetCubesLeft().ToString();
        SetColor();
    }


    public void RemoveObject()
    {
        constructionGridManager.UpdateUserTiles();
        objectsLeft.text = boundaryBoxController.GetCubesLeft().ToString();
        SetColor();
    }

    public void ShowInvalidPositionIndicator()
    {
        invalidPositionIndicator.enabled = true;
        StartCoroutine(IndicatorInvalidPositionCubeCoroutine());
    }

    public bool ObjectsAvailables()
    {
        return boundaryBoxController.GetCubesLeft() > 0;
    }

    private void SetColor()
    {
        if (boundaryBoxController.GetCubesLeft() == 0)
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

        if (boundaryBoxController.CheckCubePositions())
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

    IEnumerator IndicatorInvalidPositionCubeCoroutine()
    {
        yield return new WaitForSeconds(0.85f);
        invalidPositionIndicator.enabled = false;
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

}
