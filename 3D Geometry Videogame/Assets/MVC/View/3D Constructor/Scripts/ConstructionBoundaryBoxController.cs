using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstructionBoundaryBoxController : MonoBehaviour
{
    public List<Vector3> cubePositions;

    public GameObject cubePrefab;

    private ConstructionController constructionController;

    private ConstructionCameraController scriptCamera;

    void Awake()
    {

        scriptCamera = GameObject.Find("Main Camera").GetComponent<ConstructionCameraController>();

        constructionController = ConstructionController.Instance;
        constructionController.SetUpValues();
        constructionController.GetTargetTileMatrices();

        cubePositions = new List<Vector3>();
        GenerateStartingScenario();
        
        constructionController.GetUserTileMatrices(cubePositions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateStartingScenario()
    {
        var random = new System.Random();
        Vector3 randomStartPos = constructionController.targetCubePositions[random.Next(constructionController.targetCubePositions.Count)];

        cubePositions.Add(randomStartPos);
        transform.position = randomStartPos;

        GameObject newCube = Instantiate(cubePrefab, randomStartPos, Quaternion.identity);
        newCube.transform.parent = gameObject.transform;

        scriptCamera.SetStartingCameraPos(randomStartPos, new Vector3(0, 0, -3));
        SendBounds();
    }

    public int GetCubesLeft()
    {
        return constructionController.GetNumberOfCubes() - cubePositions.Count;
    }

    public void AddNewObject(Vector3 newCubePosition, Transform newCube)
    {
        cubePositions.Add(newCubePosition);
        newCube.parent = transform;
        UpdateUserTiles();
    }

    public void RemoveObject(Vector3 newCubePosition)
    {
        cubePositions.Remove(newCubePosition);
        UpdateUserTiles();
    }

    private void UpdateUserTiles()
    {
        constructionController.GetUserTileMatrices(cubePositions);

    }

    public bool CheckCubePositions()
    {
        return constructionController.CheckCubePositions(cubePositions);
    }

    // ------------------------------------------------ BOUNDS SETTER ------------------------------------------------


    public void SendBounds()
    {
        Bounds bounds = GetBounds(gameObject);
        scriptCamera.currentVRP = bounds.center;
        scriptCamera.boxBounds = bounds;
    }


    // ------------------------------------------------ BOUNDS GENERATORS ------------------------------------------------


    private Bounds GetBounds(GameObject boundaryBox)
    {
        Bounds bounds;
        Renderer childRender;
        bounds = GetRenderBounds(boundaryBox);
        if (bounds.extents.x == 0)
        {
            bounds = new Bounds(boundaryBox.transform.position, Vector3.zero);
            foreach (Transform child in boundaryBox.transform)
            {
                childRender = child.GetComponent<Renderer>();
                if (childRender)
                {
                    bounds.Encapsulate(childRender.bounds);
                }
                else
                {
                    bounds.Encapsulate(GetBounds(child.gameObject));
                }
            }
        }
        return bounds;
    }

    private Bounds GetRenderBounds(GameObject boundaryBox)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        Renderer render = boundaryBox.GetComponent<Renderer>();
        if (render != null)
        {
            return render.bounds;
        }
        return bounds;
    }
}
