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
    private Vector3 cameraPosAux; //Aux Camera position in global coordinates

    private Vector3 closestPoint; //Closest point of the Bounding Box to the Main Camera position


    // ------------------------------------------------ INITIALIZATIONS ------------------------------------------------


    void Start()
    {

        currentVRP = new Vector3(0, 0, 0);
        boxBounds = new Bounds();
        cameraPos = new Vector3(0, 0, 0);
        cameraPosAux = GameObject.Find("Aux Camera").transform.position;
        closestPoint = new Vector3(0, 0, 0);

    }


    // ------------------------------------------------ UPDATING METHODS PER FRAME ------------------------------------------------

    private void Update()
    {

        //---- TRANSLATION ----
        Vector3 translation = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) translation += transform.forward;
        if (Input.GetKey(KeyCode.A)) translation -= transform.right;
        if (Input.GetKey(KeyCode.S)) translation -= transform.forward;
        if (Input.GetKey(KeyCode.D)) translation += transform.right;
        if (Input.GetKey(KeyCode.Q)) translation += Vector3.up;
        if (Input.GetKey(KeyCode.E)) translation -= Vector3.up;

        transform.Translate(translation * Time.fixedDeltaTime * 0.80f, Space.World);

        //---- ZOOM ----

        cameraPos = transform.position;
        closestPoint = boxBounds.ClosestPoint(cameraPos);
        float distance = Vector3.Distance(cameraPos, closestPoint);
        Vector3 repositionDir = currentVRP - cameraPos;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (distance >= 2) transform.Translate(Vector3.Normalize(repositionDir) * 0.5f, Space.World); //Translating the Main Camera to VRP with magnitude 0.5 
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (distance <= 5) transform.Translate(Vector3.Normalize(-repositionDir) * 0.5f, Space.World); //Moving the Main Camera away from VRP with magnitude 0.5 
        }

        bool leftAltClicked = Input.GetKey(KeyCode.LeftAlt);
        bool mouseLeftClicked = Input.GetMouseButton(0);
        bool mouseRightClicked = Input.GetMouseButton(1);

        //---- ROTATION ----
        Vector3 rotation = Vector3.zero;
        if (mouseRightClicked)
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            rotation.y = horizontalInput;
            rotation.x = -verticalInput;

            transform.Rotate(rotation * Time.fixedDeltaTime * speed);
        }

        //---- ORBIT ----
        if (leftAltClicked && mouseLeftClicked)
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            transform.RotateAround(currentVRP, transform.up, horizontalInput * Time.fixedDeltaTime * speed);
            transform.RotateAround(currentVRP, transform.right, -verticalInput * Time.fixedDeltaTime * speed);
        }

        //---- Debugging tools ----
        Debug.DrawRay(cameraPos, closestPoint - cameraPos, Color.green);
        Debug.DrawRay(cameraPos, currentVRP - cameraPos, Color.blue);

    }


    public void SetCamera()
    {
        transform.position = cameraPosAux;
        transform.rotation = Quaternion.LookRotation(currentVRP - transform.position);
    }
}
