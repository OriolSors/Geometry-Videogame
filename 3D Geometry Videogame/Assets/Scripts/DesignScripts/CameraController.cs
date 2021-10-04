using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;

    public Vector3 cameraDir;
    void Start()
    {
        cameraDir = transform.forward;

    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(transform.right * speed * horizontalInput * Time.fixedDeltaTime, Space.World);
        transform.Translate(transform.up * speed * verticalInput * Time.fixedDeltaTime, Space.World);

    }

    public void RotateUp()
    {
        transform.Translate(transform.up * 2, Space.World);
        transform.Rotate(transform.right, 90, Space.World);
        cameraDir = transform.forward;

    }

    public void RotateDown()
    {
        transform.Translate(-transform.up * 2, Space.World);
        transform.Rotate(transform.right, -90, Space.World);
        cameraDir = transform.forward;

    }

    public void RotateRight()
    {
        transform.Translate(transform.right * 2, Space.World);
        transform.Rotate(transform.up, -90, Space.World);
        cameraDir = transform.forward;

    }

    public void RotateLeft()
    {
        transform.Translate(-transform.right * 2, Space.World);
        transform.Rotate(transform.up, 90, Space.World);
        cameraDir = transform.forward;

    }
}
