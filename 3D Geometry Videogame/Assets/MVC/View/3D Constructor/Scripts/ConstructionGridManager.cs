using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionGridManager : MonoBehaviour
{
    public GameObject matrixTile;
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
                SpawnConcreteTile(i, j, M_x_y[i, j], "xy");
                SpawnConcreteTile(i, j, M_z_y[i, j], "zy");
                SpawnConcreteTile(i, j, M_x_z[i, j], "xz");

                SpawnConcreteTile(i, j, uM_x_y[i, j], "u_xy");
                SpawnConcreteTile(i, j, uM_z_y[i, j], "u_zy");
                SpawnConcreteTile(i, j, uM_x_z[i, j], "u_xz");
            }
        }
    }

    private void SpawnUserTiles()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                SpawnConcreteTile(i, j, uM_x_y[i, j], "u_xy");
                SpawnConcreteTile(i, j, uM_z_y[i, j], "u_zy");
                SpawnConcreteTile(i, j, uM_x_z[i, j], "u_xz");
            }
        }
    }

    private void SpawnConcreteTile(int i, int j, int sum_k, string projection)
    {
        Transform currentMatrix = null;
        GameObject go = Instantiate(matrixTile);
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
            case "u_xy":
                currentMatrix = uMatrixGridXY;
                break;
            case "u_zy":
                currentMatrix = uMatrixGridZY;
                break;
            case "u_xz":
                currentMatrix = uMatrixGridXZ;
                break;
        }

        
        go.transform.SetParent(currentMatrix);
        go.transform.localPosition = new Vector2(-width/2 + tile_width/2 + tile_width*j, height/2 - tile_height / 2 - tile_height * i);
        go.GetComponentInChildren<TextMeshProUGUI>().text = sum_k.ToString();
        if (sum_k == 0) go.GetComponent<Image>().color = new Color(219, 215, 231);
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
