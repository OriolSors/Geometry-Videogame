using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionGridManager : MonoBehaviour
{
    public GameObject doneMatrixTile, workingMatrixTile, wrongMatrixTile, overflowMatrixTile, noMatrixTile;
    private float width, height;

    private int N;
    private int[,] M_x_y, M_z_y, M_x_z;
    private int[,] uM_x_y, uM_z_y, uM_x_z;

    public Transform matrixGridXY, matrixGridZY, matrixGridXZ;
    public Transform uMatrixGridXY, uMatrixGridZY, uMatrixGridXZ;

    public void InitializeValues()
    {
        var rectTransform = matrixGridXY.GetComponent<RectTransform>();
        width = rectTransform.sizeDelta.x;
        height = rectTransform.sizeDelta.y;

        N = ConstructionController.Instance.N;

        M_x_y = ConstructionController.Instance.M_x_y;
        M_z_y = ConstructionController.Instance.M_z_y;
        M_x_z = ConstructionController.Instance.M_x_z;

        uM_x_y = ConstructionController.Instance.uM_x_y;
        uM_z_y = ConstructionController.Instance.uM_z_y;
        uM_x_z = ConstructionController.Instance.uM_x_z;
    }


    public void SpawnTiles()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                SpawnTargetTile(i, j, M_x_y[i, j], "xy");
                SpawnTargetTile(i, j, M_z_y[i, j], "zy");
                SpawnTargetTile(i, j, M_x_z[i, j], "xz");

                SpawnCurrentTile(i, j, uM_x_y[i, j], "u_xy");
                SpawnCurrentTile(i, j, uM_z_y[i, j], "u_zy");
                SpawnCurrentTile(i, j, uM_x_z[i, j], "u_xz");
            }
        }
    }

    private void SpawnUserTiles()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                SpawnCurrentTile(i, j, uM_x_y[i, j], "u_xy");
                SpawnCurrentTile(i, j, uM_z_y[i, j], "u_zy");
                SpawnCurrentTile(i, j, uM_x_z[i, j], "u_xz");
            }
        }
    }

    private void SpawnTargetTile(int i, int j, int sum_k, string projection)
    {
        Transform currentMatrix = null;
        GameObject go = Instantiate(doneMatrixTile);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(width / N, height / N);
        float tile_width = go.GetComponent<RectTransform>().sizeDelta.x;
        float tile_height = go.GetComponent<RectTransform>().sizeDelta.y;

        switch (projection)
        {
            case "xy":
                currentMatrix = matrixGridXY;
                break;
            case "zy":
                currentMatrix = matrixGridZY;
                break;
            case "xz":
                currentMatrix = matrixGridXZ;
                break;
        }

        
        go.transform.SetParent(currentMatrix);
        go.transform.localPosition = new Vector2(-width/2 + tile_width/2 + tile_width*j, height/2 - tile_height / 2 - tile_height * i);
        go.GetComponentInChildren<TextMeshProUGUI>().text = sum_k.ToString();
        if (sum_k == 0) go.GetComponent<Image>().color = new Color(219, 215, 231);
    }

    private void SpawnCurrentTile(int i, int j, int sum_k, string projection)
    {
        Transform currentMatrix = null;
        GameObject go = null;
        switch (projection)
        {
            case "u_xy":
                currentMatrix = uMatrixGridXY;
                go = ChooseTileStyle(i, j, sum_k, M_x_y, go);

                //LOG
                Globals.logBuffer.Append("Computing position M_" + i + "_" + j + " at projection XY" + "\n");
                Globals.logBuffer.Append("\n");
                break;
            case "u_zy":
                currentMatrix = uMatrixGridZY;
                go = ChooseTileStyle(i, j, sum_k, M_z_y, go);

                //LOG
                Globals.logBuffer.Append("Computing position M_" + i + "_" + j + " at projection ZY" + "\n");
                Globals.logBuffer.Append("\n");
                break;
            case "u_xz":
                currentMatrix = uMatrixGridXZ;
                go = ChooseTileStyle(i, j, sum_k, M_x_z, go);

                //LOG
                Globals.logBuffer.Append("Computing position M_" + i + "_" + j + " at projection XZ" + "\n");
                Globals.logBuffer.Append("\n");
                break;
        }

        go.GetComponent<RectTransform>().sizeDelta = new Vector2(width / N, height / N);
        float tile_width = go.GetComponent<RectTransform>().sizeDelta.x;
        float tile_height = go.GetComponent<RectTransform>().sizeDelta.y;

       
        go.transform.SetParent(currentMatrix);
        go.transform.localPosition = new Vector2(-width / 2 + tile_width / 2 + tile_width * j, height / 2 - tile_height / 2 - tile_height * i);
        go.GetComponentInChildren<TextMeshProUGUI>().text = sum_k.ToString();
    }

    private GameObject ChooseTileStyle(int i, int j, int sum_k, int[,] m, GameObject go)
    {

        if (m[i, j] == 0 && sum_k != 0)
        {
            go = Instantiate(wrongMatrixTile);

            //LOG
            Globals.wrongPositionCount += 1;
            Globals.logBuffer.Append("\n");
            Globals.logBuffer.Append("[WRONG] cube position. Count: " + Globals.wrongPositionCount + "\n");
            Globals.logBuffer.Append("\n");
        }
        else if (sum_k == 0)
        {
            go = Instantiate(noMatrixTile);
        }
        else if (m[i, j] == sum_k)
        {
            go = Instantiate(doneMatrixTile);
        }
        else if (m[i, j] > sum_k)
        {
            go = Instantiate(workingMatrixTile);
        }
        else if (m[i, j] < sum_k)
        {
            go = Instantiate(overflowMatrixTile);

            //LOG
            Globals.overflowPositionCount += 1;
            Globals.logBuffer.Append("\n");
            Globals.logBuffer.Append("[OVERFLOW] cube position. Count: " + Globals.overflowPositionCount + "\n");
            Globals.logBuffer.Append("\n");
        }

        return go;
    }

    public void UpdateUserTiles()
    {
        uM_x_y = ConstructionController.Instance.uM_x_y;
        uM_z_y = ConstructionController.Instance.uM_z_y;
        uM_x_z = ConstructionController.Instance.uM_x_z;

        foreach (Transform child in uMatrixGridXY)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in uMatrixGridZY)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in uMatrixGridXZ)
        {
            Destroy(child.gameObject);
        }

        SpawnUserTiles();
    }
}
