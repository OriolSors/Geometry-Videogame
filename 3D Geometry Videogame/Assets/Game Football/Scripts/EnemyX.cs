using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed = 30;
    private Rigidbody enemyRb;
    private GameObject playerGoal;
    private SpawnManagerX spawnScript;

    private bool stopped = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        spawnScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerX>();
        speed += spawnScript.increaseSpeed;
        playerGoal = GameObject.Find("Player Goal");
    }

    // Update is called once per frame
    void Update()
    {
        // Set enemy direction towards player goal and move there
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        if(!stopped)enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
        else enemyRb.AddForce(Vector3.zero);

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

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            spawnScript.NewGoalPlayer();
            Destroy(gameObject);
        } 
        else if (other.gameObject.name == "Player Goal")
        {
            spawnScript.NewGoalEnemy();
            Destroy(gameObject);
        }

    }

}
