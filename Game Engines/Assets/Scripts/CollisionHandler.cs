using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public BackPack myBackpack;
    private Vector3 spawnPlayer;
    private bool hitObstacle = false;
    // Start is called before the first frame update
    void Start()
    {
        //store the player's initial position 
        spawnPlayer = transform.position;
        // Initialize the instance (this calls the constructor)
        myBackpack = new BackPack(100); // You can specify a custom capacity if needed
        // Use the instance to call methods
        myBackpack.AddToBackpack();
        StartCoroutine(CheckObstacleTimer());
    }
    
    IEnumerator CheckObstacleTimer()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        
        if (!hitObstacle)
        {
            Debug.Log("5 seconds passed and no obstacle was hit!");
            
        }
    }
    void OnCollisionEnter(Collision collision)
    {
    
        // Code to handle player collision with an enemy, such as reducing health or ending the game
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with an enemy!");
            RespawnPlayer();

        }
        // Check if the player collided with an obstacle
        if (collision.gameObject.CompareTag("Obstacle"))    
        {
            Debug.Log("Player hit an obstacle!");
            hitObstacle = true; // Set the flag to true if an obstacle is hit
            RespawnPlayer();
        }
      
        
    }

    // Change this method name and the parameter type (Collision -> Collider)
    void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Touched something named: " + other.name + " with Tag: " + other.tag);
        // Check if the player collided with a coin
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Player picked up a coin!");
            myBackpack.AddToBackpack();
            
            if (CollectiblesManager.Instance != null)
            {
                CollectiblesManager.Instance.AddCoins(1);
            }
            
            Destroy(other.gameObject); 
        }


        if (other.CompareTag("SpeedBooster"))
        {
            Debug.Log("Speed boost collected!");
            Destroy(other.gameObject);
        }
        
    }
   


    //variable to store the players initial position or a respawn position
    void RespawnPlayer()
    {
        //If there is a Rigidbody, reset its velocity so i restart properly without any residual movement from the collision
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (!rb.isKinematic)
            {
                // Reset movement
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
           
        }
        // this function  reset the player back to its initial position or a designated respawn point after colliding with an enemy
        transform.position = spawnPlayer; //store the players initial position or a respawn position
        // Forces the physics engine to acknowledge the move immediately
        Physics.SyncTransforms();
        Debug.Log("Teleported to: " + spawnPlayer);
        // 1. Declare the variable at the top of your class
    

    }
}

