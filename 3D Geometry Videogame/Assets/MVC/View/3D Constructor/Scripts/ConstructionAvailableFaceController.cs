using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionAvailableFaceController : MonoBehaviour
{

    // ------------------------------------------------ DECLARATIONS ------------------------------------------------


    public GameObject cubePrefab; //Cube prefab

    private ConstructionCanvasManager canvasManager; //The Canvas script

    private ConstructionBoundaryBoxController boundaryBoxController; //The Boundary Box controller


    // ------------------------------------------------ INITIALIZATIONS ------------------------------------------------


    void Start()
    {

        canvasManager = GameObject.Find("Canvas").GetComponent<ConstructionCanvasManager>();
        boundaryBoxController = GameObject.Find("Boundary Box").GetComponent<ConstructionBoundaryBoxController>();

    }


    // ------------------------------------------------ MOUSE CLICK LISTENERS ------------------------------------------------
    private void OnMouseOver()
    {
        //Left click on mouse

        if (Input.GetMouseButtonDown(0))
        {
            
            // Ray Casting to find the available face to add the new object

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.rigidbody != null && canvasManager.ObjectsAvailables()) 
                {
                    //Adds new Cube prefab to the normal direction of face clicked

                    GameObject newCube = Instantiate(cubePrefab, hit.transform.position + 0.5f*hit.normal, Quaternion.identity) as GameObject; //TODO: for all the other Platonic Solids, create new methods to scale and position the object

                    Vector3 newCubePosition = Round(newCube.transform.position);

                    if (!boundaryBoxController.cubePositions.Contains(newCubePosition) && ConstructionController.Instance.IsValidPosition(newCubePosition)) 
                    {

                        //If there is not an object at the new position, we add it

                        boundaryBoxController.AddNewObject(newCubePosition, newCube.transform);
                        canvasManager.AddNewObject();

                        //Sending Bounding Box bounds and VRP to the Camera Controller

                        boundaryBoxController.SendBounds();

                    }
                    else if(boundaryBoxController.cubePositions.Contains(newCubePosition))
                    {

                        //If there is an object at the new position, we destroy it instantly

                        Destroy(newCube); 

                    }
                    else
                    {
                        Destroy(newCube);
                        canvasManager.ShowInvalidPositionIndicator();
                    }
                    
                }
            }

        }

        //Right click on mouse

        if (Input.GetMouseButtonDown(1))
        {
            if (boundaryBoxController.cubePositions.Count > 1)
            {
                boundaryBoxController.RemoveObject(Round(gameObject.transform.position));
                canvasManager.RemoveObject();
                Destroy(gameObject);
            }


            //Sending Bounding Box bounds and VRP to the Camera Controller+

            boundaryBoxController.SendBounds();

        }
        

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
