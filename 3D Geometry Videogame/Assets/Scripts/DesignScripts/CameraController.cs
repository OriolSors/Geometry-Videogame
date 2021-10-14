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

    public Vector3 currentVRP;

    public Vector3 upVRP;
    public Vector3 downVRP;
    public Vector3 rightVRP;
    public Vector3 leftVRP;
    public Vector3 forwardVRP;
    public Vector3 backVRP;



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
        currentVRP = new Vector3(0, 0, 0); //TODO: llista amb els valors mes alts a UP, -UP, RIGHT, -RIGHT, FORWARD, -FORWARD i en els m?todes de Rotate seleccionar el VRP m?s adequat
        upVRP = new Vector3(0, 0, 0);
        downVRP = new Vector3(0, 0, 0);
        rightVRP = new Vector3(0, 0, 0);
        leftVRP = new Vector3(0, 0, 0);
        forwardVRP = new Vector3(0, 0, 0);
        backVRP = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(transform.right * speed * horizontalInput * Time.fixedDeltaTime, Space.World);
        transform.Translate(transform.up * speed * verticalInput * Time.fixedDeltaTime, Space.World);

        if (activeUp)
        {
            directionToFace = currentVRP - transform.position;
            rotationAngle = Vector3.Angle(initialDirection, directionToFace);
            
            RotateUp();
        }
        if (activeDown)
        {
            directionToFace = currentVRP - transform.position;
            rotationAngle = Vector3.Angle(initialDirection, directionToFace);
            
            RotateDown();
        }
        if (activeRight)
        {
            directionToFace = currentVRP - transform.position;
            rotationAngle = Vector3.Angle(initialDirection, directionToFace);
            
            RotateRight();
        }
        if (activeLeft)
        {
            directionToFace = currentVRP - transform.position;
            rotationAngle = Vector3.Angle(initialDirection, directionToFace);
            
            RotateLeft();
        }

    }

    public void RotateUp()
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
        else
        {
            transform.RotateAround(currentVRP, transform.right, 90 * Time.deltaTime);

            cameraDir = Vector3Int.RoundToInt(transform.forward);
        }

        

        

    }


    public void RotateDown()
    {

        if (rotationAngle >= 90)
        {
            activeDown = false;
            
        }
        else
        {
            transform.RotateAround(currentVRP, -transform.right, 90 * Time.deltaTime);

            cameraDir = Vector3Int.RoundToInt(transform.forward);
        }

        

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
        else
        {
            transform.RotateAround(currentVRP, -transform.up, 90 * Time.deltaTime);

            cameraDir = Vector3Int.RoundToInt(transform.forward);
        }

        

    }

    public void RotateLeft()
    {
        if (rotationAngle >= 90)
        {
            activeLeft = false;
            
        }
        else
        {
            transform.RotateAround(currentVRP, transform.up, 90 * Time.deltaTime);

            cameraDir = Vector3Int.RoundToInt(transform.forward);
        }

        
    }



    private Vector3 GetCorrectVRP(Vector3 dir)
    {
        if (dir == Vector3.up)
        {
            currentVRP = upVRP;
        }
        else if (dir == -Vector3.up)
        {
            currentVRP = downVRP;
        }
        else if (dir == Vector3.forward)
        {
            currentVRP = forwardVRP;
        }
        else if (dir == -Vector3.forward)
        {
            currentVRP = backVRP;
        }
        else if (dir == Vector3.right)
        {
            currentVRP = rightVRP;
        }
        else //if (dir == -Vector3.right)
        {
            currentVRP = leftVRP;
        }

        return currentVRP;
    }


    public void ActiveRotateUp()
    {
        activeUp = true;
        initialDirection = GetCorrectVRP(transform.up) - transform.position;
    }

    public void ActiveRotateDown()
    {
        activeDown = true;
        initialDirection = GetCorrectVRP(-transform.up) - transform.position;
    }

    public void ActiveRotateRight()
    {
        activeRight = true;
        initialDirection = GetCorrectVRP(transform.right) - transform.position;
    }

    public void ActiveRotateLeft()
    {
        activeLeft = true;
        initialDirection = GetCorrectVRP(-transform.right) - transform.position;
    }

}
