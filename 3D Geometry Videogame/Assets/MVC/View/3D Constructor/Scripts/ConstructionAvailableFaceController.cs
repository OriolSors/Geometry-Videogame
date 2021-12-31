using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionAvailableFaceController : MonoBehaviour
{

    // ------------------------------------------------ DECLARATIONS ------------------------------------------------


    public GameObject cubePrefab; //Cube prefab

    private GameObject boundaryBox; //Parent Bounding Box that englobes all the construction

    private ConstructionCameraController scriptCamera; //The Main Camera script

    private ConstructionCanvasManager canvasManager; //The Construction script


    // ------------------------------------------------ INITIALIZATIONS ------------------------------------------------


    void Start()
    {

        scriptCamera = GameObject.Find("Main Camera").GetComponent<ConstructionCameraController>();
        canvasManager = GameObject.Find("Boundary Box").GetComponent<ConstructionCanvasManager>();
        boundaryBox = GameObject.Find("Boundary Box");

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

                    Vector3 newCubePosition = newCube.transform.position;

                    if (!canvasManager.cubePositions.Contains(newCubePosition)) 
                    {

                        //If there is not an object at the new position, we add it

                        canvasManager.AddNewObject(newCubePosition);
                        newCube.transform.parent = boundaryBox.transform; 

                        //Sending Bounding Box bounds and VRP to the Camera Controller

                        SendBounds();

                    }
                    else
                    {

                        //If there is an object at the new position, we destroy it instantly

                        Destroy(newCube); 

                    }
                    
                }
            }

        }

        //Right click on mouse

        if (Input.GetMouseButtonDown(1))
        {
            if (canvasManager.cubePositions.Count > 1)
            {
                canvasManager.RemoveObject(gameObject.transform.position);
                Destroy(gameObject);
            }
            

            //Sending Bounding Box bounds and VRP to the Camera Controller+

            SendBounds();

        }
        

    }


    // ------------------------------------------------ BOUNDS SETTER ------------------------------------------------


    private void SendBounds()
    {
        Bounds bounds = GetBounds(boundaryBox);
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
