using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System;

public class InventorySelection : MonoBehaviour
{

    public RectTransform inventory;
    public GameObject buttonSelected;
    

    // Start is called before the first frame update
    void Start()
    {
        
        GameObject button = Instantiate(buttonSelected) as GameObject;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Cube";
        button.transform.SetParent(inventory, false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
}
