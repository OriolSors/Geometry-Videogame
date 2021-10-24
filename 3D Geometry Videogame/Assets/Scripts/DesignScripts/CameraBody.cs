using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBody : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    // Update is called once per frame
    void Update()
    {
        transform.position = mainCamera.transform.position;
    }
}
