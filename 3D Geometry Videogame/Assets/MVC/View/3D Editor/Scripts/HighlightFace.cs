using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightFace : MonoBehaviour
{
    [SerializeField]
    private Material highlightGreenMaterial;

    [SerializeField]
    private Material highlightRedMaterial;

    [SerializeField]
    private Material defaultMaterial;

    private CanvasManager canvasManager; //The Construction script

    private void Start()
    {
        canvasManager = GameObject.Find("Boundary Box").GetComponent<CanvasManager>();
    }

    private void Update()
    {
        // Ray Casting to find the available face
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (canvasManager.ObjectsAvailables())
                {
                    gameObject.GetComponent<MeshRenderer>().material = highlightGreenMaterial;

                }
                else 
                {
                    gameObject.GetComponent<MeshRenderer>().material = highlightRedMaterial;
                }

            }else
            {
                gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
            }

        }
    }
}
