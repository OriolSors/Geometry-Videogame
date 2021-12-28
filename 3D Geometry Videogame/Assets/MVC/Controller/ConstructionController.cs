using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstructionController 
{
    private List<Vector3> cubePositions = new List<Vector3>();

    private float max_pos_x, max_pos_y, max_pos_z;
    private float min_pos_x, min_pos_y, min_pos_z;
    private float max_value, min_value;
    private int N;

    private int[,] M_x_y, M_z_y, M_x_z;

    private ConstructionController()
    {
        cubePositions = MissionListController.Instance.GetCurrentMissionPlayer().GetCubePositions();
        SetUpValues();
    }

    private static ConstructionController instance = null;
    public static ConstructionController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConstructionController();
            }
            return instance;
        }
    }

    public int GetNumberOfCubes()
    {
        return cubePositions.Count();
    }


    public void SetUpValues()
    {
        max_pos_x = cubePositions.Max(v => v.x);
        min_pos_x = cubePositions.Min(v => v.x);

        max_pos_y = cubePositions.Max(v => v.y);
        min_pos_y = cubePositions.Min(v => v.y);

        max_pos_z = cubePositions.Max(v => v.z);
        min_pos_z = cubePositions.Min(v => v.z);

        max_value = (new float[] { max_pos_x, max_pos_y, max_pos_z }).Max();
        min_value = (new float[] { min_pos_x, min_pos_y, min_pos_z }).Min();

        Debug.Log("max: " + max_value);
        Debug.Log("min: " + min_value);

        N = Convert.ToInt32((max_value - min_value )*2 + 1);
        Debug.Log("N: " + N);

        M_x_y = new int[N, N];
        M_z_y = new int[N, N];
        M_x_z = new int[N, N];
    }

    public void LoadTargetFigure()
    {
        for(int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                int sum_z = 0;

                foreach (Vector3 pos in cubePositions)
                {
                    float m_x = x / 2f + min_value;
                    float m_y = y / 2f + min_value;

                    if (pos.x == m_x && pos.y == m_y)
                    {
                        sum_z++;
                    }
                }

                M_x_y[x, y] = sum_z;
                Debug.Log("M" + x + y + " = " + sum_z);
            }
        }

        for (int y = 0; y < N; y++)
        {
            for (int z = 0; z < N; z++)
            {
                int sum_x = 0;

                foreach (Vector3 pos in cubePositions)
                {
                    float m_z = z / 2f + min_value;
                    float m_y = y / 2f + min_value;

                    if (pos.z == m_z && pos.y == m_y)
                    {
                        sum_x++;
                    }
                }

                M_z_y[z, y] = sum_x;
                Debug.Log("M" + z + y + " = " + sum_x);
            }
        }

        for (int z = 0; z < N; z++)
        {
            for (int x = 0; x < N; x++)
            {
                int sum_y = 0;

                foreach (Vector3 pos in cubePositions)
                {
                    float m_x = x / 2f + min_value;
                    float m_z = z / 2f + min_value;

                    if (pos.x == m_x && pos.z == m_z)
                    {
                        sum_y++;
                    }
                }

                M_x_z[x, z] = sum_y;
                Debug.Log("M" + x + z + " = " + sum_y);
            }
        }

    }

    public static Vector3 Round(Vector3 vector3, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }

    public bool CheckCubePositions(List<Vector3> cubePositions)
    {
        List<Vector3> roundedCubePositions = new List<Vector3>();
        foreach (Vector3 cubePosition in cubePositions)
        {
            roundedCubePositions.Add(Round(cubePosition, 2));
        }
        return roundedCubePositions.Except(this.cubePositions).Count() == 0 && roundedCubePositions.Count() == this.cubePositions.Count();
    }


}
