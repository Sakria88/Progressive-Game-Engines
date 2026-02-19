using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public BackPack myBackpack;
    private Vector3 spawnPlayer;
    // Start is called before the first frame update
    void Start()
    {
        //store the player's initial position 
        spawnPlayer = transform.position;
        // Initialize the instance (this calls the constructor)
        myBackpack = new BackPack(100); // You can specify a custom capacity if needed
        // Use the instance to call methods
        myBackpack.AddToBackpack();}
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I touched: " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<CollectibleItem>() != null)
        {
            myBackpack.AddToBackpack(); // Add to backpack
            Destroy(collision.gameObject); // Remove coin from world
            Debug.Log("Collected a " + collision.gameObject.name);
        }
        
        // Code to handle player collision with an enemy, such as reducing health or ending the game
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with an enemy!");
            RespawnPlayer();

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

