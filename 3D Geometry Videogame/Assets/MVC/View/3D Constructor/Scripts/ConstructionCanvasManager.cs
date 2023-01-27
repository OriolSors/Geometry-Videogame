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
    public Canvas likeAndDislikeCanvas;

    public GameObject matricesPanel;
    private ConstructionGridManager constructionGridManager;

    private ConstructionBoundaryBoxController boundaryBoxController;

    private float timer = 0.0f;
    private int finalSeconds = 0;
    private int lastTime = 0;
    private int addRemoveCount = 0;
    private bool like = false;

    void Start()
    {

        preColor = objectsLeft.color;

        constructionCorrectCanvas.enabled = false;
        constructionIncorrectCanvas.enabled = false;
        invalidPositionIndicator.enabled = false;
        likeAndDislikeCanvas.enabled = false;

        constructionGridManager = matricesPanel.GetComponent<ConstructionGridManager>();
        constructionGridManager.InitializeValues();
        constructionGridManager.SpawnTiles();

        boundaryBoxController = GameObject.Find("Boundary Box").GetComponent<ConstructionBoundaryBoxController>();
        objectsLeft.text = boundaryBoxController.GetCubesLeft().ToString();

        //LOG
        Globals.Reset();
        Globals.cubeCount = boundaryBoxController.GetCubesLeft();
        Globals.logBuffer.Append("Challenge started.\nCubes left: " + Globals.cubeCount + "\n");
        Globals.logBuffer.Append("\n");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.Tab)) matricesPanel.SetActive(true);
        else matricesPanel.SetActive(false);
        */
        timer += Time.deltaTime;

    }

    public void ResetConstruction()
    {
        //LOG
        Globals.logIntents += 1;
        Globals.logBuffer.Append("Challenge restarted.\nIntents: " + Globals.logIntents + "\n");
        Globals.logBuffer.Append("\n");

        SceneManager.LoadScene("3D Constructor");
        
    }

    public void ExitConstruction()
    {
        //LOG
        Globals.logBuffer.Append("Challenge quit" + "\n");
        Globals.logBuffer.Append("\n");

        SceneManager.LoadScene("Player Mission List Screen");

    }



    public void AddNewObject()
    {
        int currentTime = (int)(timer);
        int stepTime = currentTime - lastTime;
        lastTime = currentTime;
        addRemoveCount++;

        //LOG
        Globals.cubeCount = boundaryBoxController.GetCubesLeft();
        Globals.logBuffer.Append("Cube added.\nCubes left: " + Globals.cubeCount + "\n");
        Globals.logBuffer.Append("\n");
        Globals.logBuffer.Append("Time spent from last move: " + stepTime + "\n");
        Globals.logBuffer.Append("\n");

        constructionGridManager.UpdateUserTiles();
        objectsLeft.text = boundaryBoxController.GetCubesLeft().ToString();
        SetColor();

        
    }


    public void RemoveObject()
    {
        int currentTime = (int)(timer);
        int stepTime = currentTime - lastTime;
        lastTime = currentTime;
        addRemoveCount++;

        //LOG
        Globals.cubeCount = boundaryBoxController.GetCubesLeft();
        Globals.logBuffer.Append("Cube removed.\nCubes left: " + Globals.cubeCount + "\n");
        Globals.logBuffer.Append("\n");
        Globals.logBuffer.Append("Time spent from last move: " + stepTime + "\n");
        Globals.logBuffer.Append("\n");

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

            //LOG
            Globals.logBuffer.Append("No cubes left. " + "\n");
            Globals.logBuffer.Append("\n");
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
            finalSeconds = (int)(timer);
            constructionCorrectCanvas.enabled = true;
            StartCoroutine(IndicatorCorrectConstructionCoroutine());

            //LOG
            Globals.logBuffer.Append("\n");
            Globals.logBuffer.Append("Construction completed check." + "\n");
            Globals.logBuffer.Append("Time spent: " + finalSeconds +" seconds" +"\n");
            Globals.logBuffer.Append("Average Time steps: " + finalSeconds/addRemoveCount +" seconds" +"\n");
            Globals.logBuffer.Append("Wrong positions: " + Globals.wrongPositionCount + "\n");
            Globals.logBuffer.Append("Invalid positions: " + Globals.invalidPositionCount + "\n");
            Globals.logBuffer.Append("Overflow positions: " + Globals.overflowPositionCount + "\n");
            Globals.logBuffer.Append("Restarting intents: " + Globals.logIntents + "\n");
            Globals.logBuffer.Append("\n");

            File.AppendAllText("C:/UnityGames/logs/" + (AuthController.Instance.GetCurrentUser() as Player).GetUserName() +"_log.txt", Globals.logBuffer.ToString());
            Globals.logBuffer.Clear();
        }
        else
        {
            constructionIncorrectCanvas.enabled = true;
            StartCoroutine(IndicatorIncorrectConstructionCoroutine());

            //LOG
            Globals.logChecks += 1;
            Globals.logBuffer.Append("Wrong check. Count: " + Globals.logChecks + "\n");
            Globals.logBuffer.Append("\n");
        }
        

    }

    public void SetLike()
    {
        like = true;
        SaveResult();
    }

    public void SetDislike()
    {
        like = false;
        SaveResult();
    }

    public void SaveResult()
    {
        MissionListController.Instance.GetCurrentChallengePlayer().SetLike(like);
        MissionListController.Instance.GetCurrentChallengePlayer().SetTimeCompleted(finalSeconds);
        MissionListController.Instance.UpdateChallengePlayer();
        SceneManager.LoadScene("Player Mission List Screen");
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
        if(MissionListController.Instance.GetCurrentChallengePlayer() != null) likeAndDislikeCanvas.enabled = true;
        else SceneManager.LoadScene("Player Mission List Screen");

    }

    IEnumerator IndicatorIncorrectConstructionCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        constructionIncorrectCanvas.enabled = false;
    }

}
