using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;



public class CollisionHandler : MonoBehaviour
{
    public BackPack myBackpack;

    private Vector3 spawnPlayer;
    private PlayerController player;
    private Rigidbody rb;
    public InfiniteFloor[] floors;
    private bool hitObstacle = false;
    public EnemyController enemyController;

    void Start()
    {
        player = GetComponent<PlayerController>();
        spawnPlayer = transform.position;   

        myBackpack = new BackPack(100);
        myBackpack.AddToBackpack();
        StartCoroutine(CheckObstacleTimer());
    }

    IEnumerator CheckObstacleTimer()
    {
        yield return new WaitForSeconds(5f);

        if (!hitObstacle)
            Debug.Log("5 seconds passed and no obstacle was hit!");
    }
   
    private IEnumerator RespawnRoutine()
    {
        yield return null; // wait 1 frame
        RespawnPlayer();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Grounded for jump (only if you tag your floor "Ground")
        

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with an enemy!");
            StartCoroutine(RespawnRoutine());
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player hit an obstacle!");
            hitObstacle = true;
            RespawnPlayer();
        }
    }

    

   

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched something named: " + other.name + " with Tag: " + other.tag);

        if (other.CompareTag("Coin"))
        {
            Debug.Log("Player picked up a coin!");
            myBackpack.AddToBackpack();

            if (CollectiblesManager.Instance != null)
                CollectiblesManager.Instance.AddCoins(1);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("SpeedBooster"))
        {
            Debug.Log("Speed boost collected!");
            Destroy(other.gameObject);
        }
    }

    void RespawnPlayer()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = spawnPlayer;      // 
            rb.rotation = Quaternion.identity; // optional, only if you want reset rotation
        }
        else
        {
            transform.position = spawnPlayer;
        }

        Physics.SyncTransforms();
        Debug.Log("Teleported to: " + spawnPlayer);

        if (enemyController != null)
        {
            enemyController.ResetEnemy();
        }
        else
        {
            Debug.LogWarning("EnemyController reference not set!");
        }
        if (floors != null)
        {
            foreach (var floor in floors)
            {
                floor.ResetFloor();
            }
        }
    }

}
