using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{

    public List<Vector3> cubePositions;
    void Start()
    {
        cubePositions = new List<Vector3>();
        cubePositions.Add(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
