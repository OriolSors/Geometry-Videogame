using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableFaceController : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject boundaryBox;
    private CameraController scriptCamera;
    private List<Vector3> cubePositions;


    // Start is called before the first frame update
    void Start()
    {
        scriptCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        cubePositions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newCube;  //TODO: Evitar inserir un cub al mateix lloc que un altre


            // RAY CASTING TO FIND THE AVAILABLE FACE TO ADD THE NEW OBJECT

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    newCube = Instantiate(cubePrefab, hit.transform.position + 0.5f*hit.normal, Quaternion.identity) as GameObject; //TODO: crear metodes alternatius per a la resta de solids platonics -> segons l'escala de cadascun
                    if (!cubePositions.Contains(newCube.transform.position))
                    {
                        cubePositions.Add(newCube.transform.position);
                        newCube.transform.parent = boundaryBox.transform;
                        Bounds bounds = GetBounds(boundaryBox);
                        scriptCamera.currentVRP = bounds.center;
                        scriptCamera.boxBounds = bounds;
                    }
                    else
                    {
                        Destroy(newCube);
                    }
                    
                }
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
            Bounds bounds = GetBounds(boundaryBox);
            scriptCamera.currentVRP = bounds.center;
            scriptCamera.boxBounds = bounds;
        }
        

    }
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
