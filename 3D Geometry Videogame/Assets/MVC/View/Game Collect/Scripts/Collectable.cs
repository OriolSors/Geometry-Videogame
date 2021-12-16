using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    
    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -15)
        {
            Destroy(gameObject);
            if (gameObject.CompareTag("Cube") && gameObject.GetComponent<Renderer>().material.name == "Gold (Instance)")
            {
                playerController.IndicateCubeLost();

            }
        }
    }

}
