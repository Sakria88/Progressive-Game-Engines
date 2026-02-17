using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        spawnPlayer = transform.position; //store the player�s initial position 
    }
    Vector3 spawnPlayer;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I touched: " + collision.gameObject.name);
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
        

    }
}

