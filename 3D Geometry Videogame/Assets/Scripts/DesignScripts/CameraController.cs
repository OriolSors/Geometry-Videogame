using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    // ------------------------------------------------ DECLARATIONS ------------------------------------------------


    [SerializeField]
    private float speed; //Orbital rotation speed
    public Vector3 currentVRP { get; set; } //View reference point of the Main Camera
    public Bounds boxBounds { get; set; } //Bounds of the Bounding Box that englobes all the construction

    private Vector3 cameraPos; //Main Camera position in global coordinates

    private Vector3 closestPoint; //Closest point of the Bounding Box to the Main Camera position


    // ------------------------------------------------ INITIALIZATIONS ------------------------------------------------


    void Start()
    {

        currentVRP = new Vector3(0, 0, 0);
        boxBounds = new Bounds();
        cameraPos = new Vector3(0, 0, 0);
        closestPoint = new Vector3(0, 0, 0);

    }


    // ------------------------------------------------ UPDATING METHODS PER FRAME ------------------------------------------------


    private void Update()
    {
        //Recalculate the Main Camera position and local rotation at each frame

        RecalculatePos(); 

    }

    private void FixedUpdate()
    {
        //Smooth orbital rotation at 50 FPS

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.RotateAround(currentVRP, transform.up, -horizontalInput * Time.fixedDeltaTime * speed);
        transform.RotateAround(currentVRP, transform.right, verticalInput * Time.fixedDeltaTime * speed);

    }


    // ------------------------------------------------ RECALCULATING THE MAIN CAMERA POSITION / ROTATION ------------------------------------------------


    public void RecalculatePos()
    {
        //Gets the current Main Camera position and its closest point to the Bounding Box

        cameraPos = transform.position; 
        closestPoint = boxBounds.ClosestPoint(cameraPos);
        float distance = Vector3.Distance(cameraPos, closestPoint);

        //Gets the current Main Camera rotation and the target ones to the Bounding Box VRP

        Vector3 repositionDir = currentVRP - cameraPos;
        Quaternion current = transform.rotation;
        Quaternion target = Quaternion.LookRotation(repositionDir, transform.up);
        
        //Spherically interpolates both Quaternions in order to reorientate the Main Camera to the VRP

        transform.localRotation = Quaternion.Slerp(current, target, Time.deltaTime);

        if (distance < 1) transform.position = Vector3.Slerp(cameraPos, cameraPos - Vector3.Normalize(repositionDir) * 0.5f, Time.deltaTime * 5);

        //Zoom IN

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (distance >= 2) transform.Translate(Vector3.Normalize(repositionDir) * 0.5f, Space.World); //Translating the Main Camera to VRP with magnitude 0.5 
        } 

        //Zoom OUT

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            repositionDir = -repositionDir;
            if (distance <= 5) transform.Translate(Vector3.Normalize(repositionDir) * 0.5f, Space.World); //Moving the Main Camera away from VRP with magnitude 0.5 
        }

        //Debugging tools
        Debug.DrawRay(cameraPos, closestPoint - cameraPos, Color.green);
        Debug.DrawRay(cameraPos, currentVRP - cameraPos, Color.blue);

    }
}
