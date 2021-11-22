using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb; 
    private GameObject player;
    public float speed;

    private bool stopped = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        if (!stopped) enemyRb.AddForce(lookDirection * speed);
        else enemyRb.AddForce(Vector3.zero);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void StopMovement()
    {
        stopped = true;
        enemyRb.velocity = Vector3.zero;
        enemyRb.angularVelocity = Vector3.zero;
    }

    public void RestartMovement()
    {
        stopped = false;
    }
}
