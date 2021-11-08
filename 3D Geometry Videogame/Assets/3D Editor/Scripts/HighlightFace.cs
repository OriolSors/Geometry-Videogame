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

    private ConstructionController scriptConstruction; //The Construction script

    private void Start()
    {
        scriptConstruction = GameObject.Find("Boundary Box").GetComponent<ConstructionController>();
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
                if (scriptConstruction.ObjectsAvailables())
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
