using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstructionController 
{
    public List<Vector3> targetCubePositions = new List<Vector3>();

    public float max_pos_x, max_pos_y, max_pos_z;
    public float min_pos_x, min_pos_y, min_pos_z;
    public float max_diff;

    public int N;
    public int[,] M_x_y, M_z_y, M_x_z;
    public int[,] uM_x_y, uM_z_y, uM_x_z;

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
        return targetCubePositions.Count();
    }


    public void SetUpValues()
    {
        targetCubePositions = MissionListController.Instance.GetCurrentMissionPlayer().GetCubePositions();

        max_pos_x = targetCubePositions.Max(v => v.x);
        min_pos_x = targetCubePositions.Min(v => v.x);

        max_pos_y = targetCubePositions.Max(v => v.y);
        min_pos_y = targetCubePositions.Min(v => v.y);

        max_pos_z = targetCubePositions.Max(v => v.z);
        min_pos_z = targetCubePositions.Min(v => v.z);

        max_diff = (new float[] { max_pos_x-min_pos_x, max_pos_y-min_pos_y, max_pos_z-min_pos_z}).Max();

        N = Convert.ToInt32(max_diff*2 + 1);

        M_x_y = new int[N, N];
        M_z_y = new int[N, N];
        M_x_z = new int[N, N];

        uM_x_y = new int[N, N];
        uM_z_y = new int[N, N];
        uM_x_z = new int[N, N];
    }

    public bool IsValidPosition(Vector3 newCubePosition)
    {
        return (min_pos_x <= newCubePosition.x && newCubePosition.x <= max_pos_x) &&
            (min_pos_y <= newCubePosition.y && newCubePosition.y <= max_pos_y) &&
            (min_pos_z <= newCubePosition.z && newCubePosition.z <= max_pos_z);
    }

    public void GetTargetTileMatrices()
    {
        for(int y = 0; y < N; y++)
        {
            float m_y = y / 2f + min_pos_y;

            for (int x = 0; x < N; x++)
            {
                int sum_z = 0;

                float m_x = x / 2f + min_pos_x;
                
                foreach (Vector3 pos in targetCubePositions)
                {
                    
                    if (pos.x == m_x && pos.y == m_y)
                    {
                        sum_z++;
                    }
                }

                M_x_y[N-y-1, x] = sum_z;
                
            }
        }

        for (int y = 0; y < N; y++)
        {
            float m_y = y / 2f + min_pos_y;

            for (int z = 0; z < N; z++)
            {
                int sum_x = 0;

                float m_z = z / 2f + min_pos_z;

                foreach (Vector3 pos in targetCubePositions)
                {

                    if (pos.y == m_y && pos.z == m_z)
                    {
                        sum_x++;
                    }
                }

                M_z_y[N - y - 1, z] = sum_x;
                
            }
        }

        for (int z = 0; z < N; z++)
        {
            float m_z = z / 2f + min_pos_z;

            for (int x = 0; x < N; x++)
            {
                int sum_y = 0;

                float m_x = x / 2f + min_pos_x;

                foreach (Vector3 pos in targetCubePositions)
                {

                    if (pos.x == m_x && pos.z == m_z)
                    {
                        sum_y++;
                    }
                }

                M_x_z[N - z - 1, x] = sum_y;

            }
        }

    }

    public void GetUserTileMatrices(List<Vector3> userCubePositions)
    {
        for (int y = 0; y < N; y++)
        {
            float m_y = y / 2f + min_pos_y;

            for (int x = 0; x < N; x++)
            {
                int sum_z = 0;

                float m_x = x / 2f + min_pos_x;

                foreach (Vector3 pos in userCubePositions)
                {

                    if (pos.x == m_x && pos.y == m_y)
                    {
                        sum_z++;
                    }
                }

                uM_x_y[N - y - 1, x] = sum_z;
                
            }
        }

        for (int y = 0; y < N; y++)
        {
            float m_y = y / 2f + min_pos_y;

            for (int z = 0; z < N; z++)
            {
                int sum_x = 0;

                float m_z = z / 2f + min_pos_z;

                foreach (Vector3 pos in userCubePositions)
                {

                    if (pos.y == m_y && pos.z == m_z)
                    {
                        sum_x++;
                    }
                }

                uM_z_y[N - y - 1, z] = sum_x;
                
            }
        }

        for (int z = 0; z < N; z++)
        {
            float m_z = z / 2f + min_pos_z;

            for (int x = 0; x < N; x++)
            {
                int sum_y = 0;

                float m_x = x / 2f + min_pos_x;

                foreach (Vector3 pos in userCubePositions)
                {

                    if (pos.x == m_x && pos.z == m_z)
                    {
                        sum_y++;
                    }
                }

                uM_x_z[N - z - 1, x] = sum_y;
                
            }
        }

    }

    public bool CheckCubePositions(List<Vector3> userCubePositions)
    {
        //return userCubePositions.Except(targetCubePositions).Count() == 0 && userCubePositions.Count() == targetCubePositions.Count();
        return CompareMatrices(M_x_y,uM_x_y) && CompareMatrices(M_z_y,uM_z_y) && CompareMatrices(M_x_z,uM_x_z);
    }

    private bool CompareMatrices(int[,] data1, int[,] data2)
    {
        return data1.Rank == data2.Rank && Enumerable.Range(0, data1.Rank).All(dimension => data1.GetLength(dimension) == data2.GetLength(dimension)) && data1.Cast<int>().SequenceEqual(data2.Cast<int>());
    }


}
