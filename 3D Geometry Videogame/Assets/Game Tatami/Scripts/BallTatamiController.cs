using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTatamiController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;

    public bool hasPowerup;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;

    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5)
        {
            gameOver = true;
            
        }
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }

    }

    IEnumerator PowerupCountdownRoutine() {
        yield return new WaitForSeconds(7);
        powerupIndicator.gameObject.SetActive(false);
        hasPowerup = false;
    }
}
