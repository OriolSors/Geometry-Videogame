using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConstructionGridManager : MonoBehaviour
{
    public GameObject matrixTile;
    private float width, height;

    void Start()
    {

        var rectTransform = GetComponent<RectTransform>();
        width = rectTransform.sizeDelta.x;
        height = rectTransform.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnTiles()
    {
        for (int i = 0; i < ConstructionController.Instance.M_x_y.GetLength(0); i++)
        {
            for (int j = 0; j < ConstructionController.Instance.M_x_y.GetLength(1); j++)
            {
                SpawnConcreteTile(i, j, ConstructionController.Instance.M_x_y[i, j]);
            }
        }
    }

    private void SpawnConcreteTile(int i, int j, int sum_k)
    {
        GameObject go = Instantiate(matrixTile);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(width / ConstructionController.Instance.N, height / ConstructionController.Instance.N);
        float tile_width = go.GetComponent<RectTransform>().sizeDelta.x;
        float tile_height = go.GetComponent<RectTransform>().sizeDelta.y;
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector2(-width/2 + tile_width/2 + tile_width*j, height/2 - tile_height / 2 - tile_height * i);
        go.GetComponentInChildren<TextMeshProUGUI>().text = sum_k.ToString();
    }
}
