using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;

    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -6;

    private GameManager gameManager;
    public int pointValue;//To fit every kinds of object bad or good
    public ParticleSystem explosionParticle;


    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(),ForceMode.Impulse);

        transform.position = RandomSpawnPos();
        //call updatescore from gamemanager.cs
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    //destroy the targets by click
    private void OnMouseDown()
    {
        //while game is active, keep spawning objects
        if (gameManager.isGameActive)
        {
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);//if its clicked, play the explosionparticle
            gameManager.UpdateScore(pointValue);
            Destroy(gameObject);
        }
    }
    //destroy the targets if it hits the sensor
    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.CompareTag("Bad") && gameManager.isGameActive)
        {
            //if an object hits the sensor lose 1 live
            gameManager.UpdateLives(1);
            //if the live goes to 0, game over
            if(gameManager.live == 0)
            {
                gameManager.GameOver();
            }
        }
        Destroy(gameObject);
    }

    public void DestroyTarget()
    {
        if (gameManager.isGameActive)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
        }
    }
}
