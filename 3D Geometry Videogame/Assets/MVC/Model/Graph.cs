using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph
{
    List<Vector3> vertices = new List<Vector3>();
    List<List<Vector3>> edges = new List<List<Vector3>>();

    public Graph(List<Vector3> vertices)
    {
        this.vertices = vertices;
        GenerateEdges();
    }

    private void GenerateEdges()
    {
        foreach (Vector3 vertex in vertices)
        {
            foreach (Vector3 vertexAdj in vertices)
            {
                if (vertex == vertexAdj) continue;

                if (Vector3.Distance(vertex, vertexAdj) == 0.5f) edges.Add(new List<Vector3>() { vertex, vertexAdj }); //assegurar comparacio de floats
            }
        }
    }

    private bool NonManifold()
    {
        bool isNonManifold = false;
        foreach (Vector3 vertex in vertices)
        {
            foreach (Vector3 vertexAdj in vertices)
            {
                if (vertex == vertexAdj) continue;

                if (Mathf.Approximately(Vector3.Distance(vertex, vertexAdj), Mathf.Sqrt(0.5f)))
                {
                    isNonManifold = true;
                    foreach (Vector3 intermediate in vertices)
                    {
                        if (intermediate == vertexAdj || intermediate == vertex) continue;
                        if(ExistsEdge(intermediate,vertex) && ExistsEdge(intermediate, vertexAdj))
                        {
                            isNonManifold = false;
                            break;
                        }
                    }
                }
                if (isNonManifold) return true;
            }
        }
        return isNonManifold;
    }

    private bool ExistsEdge(Vector3 firstVertex, Vector3 secondVertex)
    {
        return edges.Any(c => c.SequenceEqual(new List<Vector3>() { firstVertex, secondVertex })) ||
            edges.Any(c => c.SequenceEqual(new List<Vector3>() { secondVertex, firstVertex }));
    }

    public bool IsValidGraph()
    {
        if (NonManifold()) return false;
        if (edges.Count == 0) return false;

        Dictionary<Vector3, bool> visited = new Dictionary<Vector3, bool>();
        int component_count = 0;

        foreach(Vector3 vertex in vertices)
        {
            visited[vertex] = false;
        }

        foreach (Vector3 vertex in vertices)
        {
            if (!visited[vertex])
            {
                visited = DFS(vertex, visited);
                component_count++;
            }
        }

        return component_count == 1;

    }

    private Dictionary<Vector3, bool> DFS(Vector3 vertex, Dictionary<Vector3, bool> visited)
    {
        visited[vertex] = true;
        foreach (Vector3 vertexAdj in vertices)
        {
            if (vertexAdj == vertex) continue;

            if (ExistsEdge(vertex, vertexAdj) && !visited[vertexAdj])
            {
                visited = DFS(vertexAdj, visited);
            }
        }

        return visited;
    }

 
}
