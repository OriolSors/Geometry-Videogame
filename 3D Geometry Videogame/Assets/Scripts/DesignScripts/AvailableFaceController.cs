using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableFaceController : MonoBehaviour
{
    public GameObject cubePrefab;
    private Vector3 cameraDir;
    private CameraController scriptCamera;

    private Vector3 currentVRP;

    // Start is called before the first frame update
    void Start()
    {
        scriptCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraDir = scriptCamera.cameraDir;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newCube;  //TODO: Evitar inserir un cub a sobre un altre


            if (-cameraDir == transform.up)
            {
                newCube = Instantiate(cubePrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity) as GameObject;

            }
            else if (-cameraDir == -transform.up)
            {
                newCube = Instantiate(cubePrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity) as GameObject;

            }
            else if (-cameraDir == transform.right)
            {
                newCube = Instantiate(cubePrefab, new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;

            }
            else if (-cameraDir == -transform.right)
            {
                newCube = Instantiate(cubePrefab, new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;

            }
            else if (-cameraDir == transform.forward)
            {
                newCube = Instantiate(cubePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), Quaternion.identity) as GameObject;

            }
            else //if (-cameraDir == -transform.forward)
            {
                newCube = Instantiate(cubePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), Quaternion.identity) as GameObject;

            }

            currentVRP = newCube.transform.position;

            UpdateVRP(currentVRP);

            //Vector3 translation = NewCameraPosition(scriptCamera, newCube);
            if (Vector3.Distance(scriptCamera.transform.position, currentVRP) <= 2.5)
            {
                scriptCamera.transform.Translate(-cameraDir * 0.5f, Space.World); //TODO: actualitzar acuradament la posicio de la camera ??
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
        
            


    }


    private void UpdateVRP(Vector3 currentVRP)
    {
        Vector3 upVRP = scriptCamera.upVRP;
        Vector3 downVRP = scriptCamera.downVRP;
        Vector3 rightVRP = scriptCamera.rightVRP;
        Vector3 leftVRP = scriptCamera.leftVRP;
        Vector3 forwardVRP = scriptCamera.forwardVRP;
        Vector3 backVRP = scriptCamera.backVRP;

        if (currentVRP.y > upVRP.y)
        {
            scriptCamera.upVRP = currentVRP;
        }
        if (currentVRP.y < downVRP.y)
        {
            scriptCamera.downVRP = currentVRP;
        }
        if (currentVRP.x > rightVRP.x)
        {
            scriptCamera.rightVRP = currentVRP;
        }
        if (currentVRP.x < leftVRP.x)
        {
            scriptCamera.leftVRP = currentVRP;
        }
        if (currentVRP.z > forwardVRP.z)
        {
            scriptCamera.forwardVRP = currentVRP;
        }
        if (currentVRP.z < backVRP.z)
        {
            scriptCamera.backVRP = currentVRP;
        }
    }

    private Vector3 NewCameraPosition(CameraController scriptCamera, GameObject newCube)
    {
        Vector3 cameraPos = scriptCamera.transform.localPosition;
        Vector3 cubePos = newCube.transform.localPosition;
        if (Vector3.Distance(cameraPos,cubePos) < 3)
        {
            if (cameraPos.z - cubePos.z > 0)
            {
                return new Vector3(0, 0, 3 + cubePos.z - cameraPos.z);
            }else if (cameraPos.z - cubePos.z < 0)
            {
                return new Vector3(0, 0, -3 + cubePos.z - cameraPos.z);
            }
        }
        return Vector3.zero;
    }
}
