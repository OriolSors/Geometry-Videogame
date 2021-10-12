using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableFaceController : MonoBehaviour
{
    public GameObject cubePrefab;
    private Vector3 cameraDir;
    private CameraController scriptCamera;
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

    private void OnMouseDown()
    {
        GameObject newCube;

        if (-cameraDir == transform.up)
        {
            newCube = Instantiate(cubePrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity) as GameObject;
            
        }else if (-cameraDir == -transform.up)
        {
            newCube = Instantiate(cubePrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity) as GameObject;

        }else if (-cameraDir == transform.right)
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

        scriptCamera.currentVRP = newCube.transform.position; //TODO: assignar a una llista de CurrentVRP segons l'eix
        scriptCamera.transform.Translate(0, 0, -0.5f); //TODO: actualitzar m�s acuradament la posici� de la c�mera
        Debug.Log(scriptCamera.currentVRP);


    }
}
