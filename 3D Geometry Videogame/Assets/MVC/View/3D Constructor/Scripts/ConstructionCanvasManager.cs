using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections;

public class ConstructionCanvasManager : MonoBehaviour
{

    public List<Vector3> cubePositions;
    public TextMeshProUGUI objectsLeft;
    private Color preColor;

    public Canvas constructionCorrectCanvas;
    public Canvas constructionIncorrectCanvas;

    private ConstructionController constructionController;

    void Start()
    {

        cubePositions = new List<Vector3>();
        cubePositions.Add(Vector3.zero);
        preColor = objectsLeft.color;

        constructionCorrectCanvas.enabled = false;
        constructionIncorrectCanvas.enabled = false;

        constructionController = ConstructionController.Instance;
        constructionController.SetUpValues();
        constructionController.LoadTargetFigure();
        objectsLeft.text = (constructionController.GetNumberOfCubes()-1).ToString();

    }

    public void ResetConstruction()
    {
        SceneManager.LoadScene("3D Constructor");
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

}
