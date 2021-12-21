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

    public ConstructionController()
    {
        cubePositions = MissionListController.Instance.GetCurrentMissionPlayer().GetCubePositions();
        SetUpValues();
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

        N = Convert.ToInt32((max_value - min_value) * 2);

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
                    if (pos.x == Round((x / 2) + min_value) && pos.y == Round((y / 2) + min_value))
                    {
                        sum_z++;
                    }
                }

                M_x_y[x, y] = sum_z;
                Debug.Log(sum_z);
            }
        }

        for (int z = 0; z < N; z++)
        {
            for (int y = 0; y < N; y++)
            {
                int sum_x = 0;

                foreach (Vector3 pos in cubePositions)
                {
                    if (pos.z == Round((z / 2) + min_value) && pos.y == Round((y / 2) + min_value)) sum_x++;
                }

                M_z_y[z, y] = sum_x;
            }
        }

        for (int x = 0; x < N; x++)
        {
            for (int z = 0; z < N; z++)
            {
                int sum_y = 0;

                foreach (Vector3 pos in cubePositions)
                {
                    if (pos.x == Round((x / 2) + min_value) && pos.z == Round((z / 2) + min_value)) sum_y++;
                }

                M_x_z[x, z] = sum_y;
            }
        }

    }

    public float Round(float pos_float, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return Mathf.Round(pos_float * multiplier) / multiplier;
    }


}
