using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public Vector3 currentVRP;

    public Bounds boxBounds;


    Vector3 cameraPos = new Vector3(0, 0, 0);
    Vector3 closestPoint = new Vector3(0, 0, 0);

    void Start()
    {
        currentVRP = new Vector3(0, 0, 0);
        boxBounds = new Bounds();

    }

    private void Update()
    {
        RecalculatePos();
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(cameraPos, closestPoint - cameraPos, Color.green);
        Debug.DrawRay(cameraPos, currentVRP - cameraPos, Color.blue);

        

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        transform.RotateAround(currentVRP, transform.up, -horizontalInput * Time.fixedDeltaTime * speed);
        transform.RotateAround(currentVRP, transform.right, verticalInput * Time.fixedDeltaTime * speed);
    }

    public void RecalculatePos()
    {
        cameraPos = transform.position;
        Vector3 repositionDir;
        Vector3 repositionDirNorm;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            repositionDir = currentVRP - cameraPos;
            repositionDirNorm = Vector3.Normalize(repositionDir);
            transform.Translate(repositionDirNorm * 0.5f, Space.World);
        } 
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            repositionDir = -currentVRP + cameraPos;
            repositionDirNorm = Vector3.Normalize(repositionDir);
            transform.Translate(repositionDirNorm * 0.5f, Space.World);
        }
            

        /*
        closestPoint = boxBounds.ClosestPoint(cameraPos); //TODO: Al ser un Boundary Box, per geometria sempre obtindrem un punt de la cara de la Box
        Vector3 repositionDir = closestPoint - cameraPos;
        Vector3 repositionDirNorm = Vector3.Normalize(repositionDir);
        transform.Translate(repositionDir, Space.World);
        transform.Translate(repositionDirNorm*0.5f, Space.World);
        */

        
        
        

        
    }
}
