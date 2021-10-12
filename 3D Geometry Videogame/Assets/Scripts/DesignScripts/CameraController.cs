using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    public Vector3 cameraDir { get; private set; }

    public Vector3 currentVRP;  //TODO: passar tot el objecte CUBE seria millor per tal de poder obtenir les coordenades locals ????

    private Vector3 directionToFace;
    private Vector3 initialDirection;

    private float rotationAngle = 0f;

    private bool activeUp = false;
    private bool activeDown = false;
    private bool activeRight = false;
    private bool activeLeft = false;

    void Start()
    {
        cameraDir = transform.forward;
        currentVRP = new Vector3(0, 0, 0); //TODO: llista amb els valors mes alts a UP, -UP, RIGHT, -RIGHT, FORWARD, -FORWARD i en els mètodes de Rotate seleccionar el VRP més adequat
    }

    private void Update()
    {
        directionToFace = currentVRP - transform.position;
        rotationAngle = Vector3.Angle(initialDirection, directionToFace);

        if (activeUp) RotateUp(rotationAngle);
        if (activeDown) RotateDown();
        if (activeRight) RotateRight();
        if (activeLeft) RotateLeft();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(transform.right * speed * horizontalInput * Time.fixedDeltaTime, Space.World);
        transform.Translate(transform.up * speed * verticalInput * Time.fixedDeltaTime, Space.World);

    }

    public void RotateUp(float rotationAngle)
    {
        /*
        Vector3 crossProduct = Vector3.Cross(directionToFace, Vector3.up);

        if (Vector3.Distance(crossProduct, Vector3.zero) < 0.1f && directionToFace.normalized == -Vector3.up)
        {
            activeUp = false;
        }

        Quaternion rotation = Quaternion.LookRotation(directionToFace);

        Quaternion current = transform.rotation;

        transform.rotation = Quaternion.Slerp(current, rotation, 50 * Time.deltaTime);

        transform.Translate(0, 5 * Time.deltaTime, 0);

        */

        if(rotationAngle >= 90)
        {
            activeUp = false;
        }

        transform.RotateAround(currentVRP, transform.right, 100 * Time.deltaTime);

        cameraDir = Vector3Int.RoundToInt(transform.forward);




    }

    public void RotateDown()
    {

        if (rotationAngle >= 90)
        {
            activeDown = false;
        }

        transform.RotateAround(currentVRP, -transform.right, 100 * Time.deltaTime);

        cameraDir = Vector3Int.RoundToInt(transform.forward);

    }

    public void RotateRight()
    {
        /*
        Vector3 crossProduct = Vector3.Cross(directionToFace, Vector3.right);

        if (Vector3.Distance(crossProduct, Vector3.zero) < 0.1f && directionToFace.normalized == -Vector3.right)
        {
            activeRight = false;
        }


        Quaternion rotation = Quaternion.LookRotation(directionToFace);

        Quaternion current = transform.rotation;

        transform.rotation = Quaternion.Slerp(current, rotation, 50*Time.deltaTime);

        transform.Translate(5*Time.deltaTime, 0, 0);

        */

        if (rotationAngle >= 90)
        {
            activeRight = false;
        }

        transform.RotateAround(currentVRP, -transform.up, 100 * Time.deltaTime);

        cameraDir = Vector3Int.RoundToInt(transform.forward);

    }

    public void RotateLeft()
    {
        if (rotationAngle >= 90)
        {
            activeLeft = false;
        }

        transform.RotateAround(currentVRP, transform.up, 100 * Time.deltaTime);

        cameraDir = Vector3Int.RoundToInt(transform.forward);
    }
    public void ActiveRotateUp()
    {
        activeUp = true;
        initialDirection = currentVRP - transform.position;
    }

    public void ActiveRotateDown()
    {
        activeDown = true;
        initialDirection = currentVRP - transform.position;
    }

    public void ActiveRotateRight()
    {
        activeRight = true;
        initialDirection = currentVRP - transform.position;
    }

    public void ActiveRotateLeft()
    {
        activeLeft = true;
        initialDirection = currentVRP - transform.position;
    }
}
