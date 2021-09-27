using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    public float edge;
    public bool collected;
    public PlayerInventory(Cube cube, bool collected)
    {
        this.edge = cube.edge;
        this.collected = collected;
    }
}
