using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    private ConstructionController constructionController;

    void Start()
    {
        constructionController = new ConstructionController();
        constructionController.SetUpValues();
        constructionController.LoadTargetFigure();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
