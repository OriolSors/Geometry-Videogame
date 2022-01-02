using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<Vector3> vertices = new List<Vector3>();
    List<List<Vector3>> edges = new List<List<Vector3>>();

    public Graph(List<Vector3> vertices, List<List<Vector3>> edges)
    {
        this.vertices = vertices;
        this.edges = edges;
    }

    public bool IsValidGraph()
    {
        bool aloneCube = true;
        if (edges == null) return false;
        foreach (Vector3 vertex in vertices)
        {
            foreach (List<Vector3> edge in edges)
            {
                if (edge.Contains(vertex))
                {
                    aloneCube = false;
                    break;
                }
            }
        }
        return !aloneCube;
    }
}
