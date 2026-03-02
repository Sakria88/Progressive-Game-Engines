// This script manages the infinite floor pieces in the game.
// Each floor piece will move ahead of the player when they 
//collide with it, creating the illusion of an endless path.
using System;
using UnityEngine;

public class InfiniteFloor : MonoBehaviour
{
    // Based on your Scale Z
    private float floorLength = 214.695f;
    private Vector3 startPosition;

    private void Start()
    {
        // Store original position of the parent
        startPosition = transform.parent.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Move the floor piece ahead of the player
            transform.parent.position += new Vector3(0, 0, floorLength * 2);
        }
    }

    public void ResetFloor()
    {
        transform.parent.position = startPosition;
    }

}
    