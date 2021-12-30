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

    void Start()
    {

        var rectTransform = transform.Find("Matrix Grid XY").GetComponent<RectTransform>();
        width = rectTransform.sizeDelta.x;
        height = rectTransform.sizeDelta.y;
        N = ConstructionController.Instance.N;
    }


    public void SpawnTiles()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                SpawnConcreteTile(i, j, ConstructionController.Instance.M_x_y[i, j], "xy");
                SpawnConcreteTile(i, j, ConstructionController.Instance.M_z_y[i, j], "zy");
                SpawnConcreteTile(i, j, ConstructionController.Instance.M_x_z[i, j], "xz");
            }
        }
    }

    private void SpawnConcreteTile(int i, int j, int sum_k, string projection)
    {
        Transform currentMatrix = null;
        GameObject go = Instantiate(matrixTile);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(width / ConstructionController.Instance.N, height / ConstructionController.Instance.N);
        float tile_width = go.GetComponent<RectTransform>().sizeDelta.x;
        float tile_height = go.GetComponent<RectTransform>().sizeDelta.y;

        switch (projection)
        {
            case "xy":
                currentMatrix = transform.Find("Matrix Grid XY");
                break;
            case "zy":
                currentMatrix = transform.Find("Matrix Grid ZY");
                break;
            case "xz":
                currentMatrix = transform.Find("Matrix Grid XZ");
                break;
        }
        
        go.transform.SetParent(currentMatrix);
        go.transform.localPosition = new Vector2(-width/2 + tile_width/2 + tile_width*j, height/2 - tile_height / 2 - tile_height * i);
        go.GetComponentInChildren<TextMeshProUGUI>().text = sum_k.ToString();
        if (sum_k == 0) go.GetComponent<Image>().color = new Color(219, 215, 231);
    }
}
