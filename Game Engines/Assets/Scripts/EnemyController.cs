using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //30f= 30 meters per second or frame.
    //Generally slower than player to give the player a chance to escape or fight back.
    private float moveSpeed = 5f;
    // Reference to the player's transform
    public Transform player;
    private bool isChasing = false;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        Debug.Log("Enemy spawned. Waiting 5 seconds...");
        Invoke("EnableChasing", 5f); // Start chasing after a delay of 5 seconds)
    }

    private void FixedUpdate()
    {
        if (isChasing)
        {
            if (player != null)
            {
                ChasePlayer();
            }
            else
            {
                Debug.LogWarning("Player reference is missing. Enemy cannot chase.");
            }
        }
        
    }
    private void EnableChasing()
    {
        isChasing = true;
        Debug.Log("5 seconds are up! Enemy is now chasing.");
    }


    private void ChasePlayer()
    {
        //Calculate the step based on the moveSpeed and the time between frames
        float step = moveSpeed * Time.deltaTime;
        //MoveTowards( Current Position, Target Positionb, Max Distance Delta)
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        //Make the enemy look at the player
        //transform.LookAt(player.transform.position)- specific coordinates of the player to look at
        //transform.LookAt(player) - the entire player object to look at
        transform.LookAt(player);
    }

    public void ResetEnemy()
    {
        StopAllCoroutines(); // Stop any existing wait coroutines
        isChasing = false; // Stop chasing
        transform.position = startPosition; // Reset to start
        Debug.Log("Enemy reset. Waiting 5 seconds...");
        Invoke("EnableChasing", 5f); // Wait 5 seconds before chasing again
    }
}
