using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube
{
    public float edge;

    public Cube(float edge)
    {
        this.edge = edge;
    }

    public Cube()
    {
        this.edge = Random.Range(0.5f, 1f);
    }
}