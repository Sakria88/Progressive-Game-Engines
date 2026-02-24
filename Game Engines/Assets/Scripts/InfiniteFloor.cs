using System;
using UnityEngine;

public class InfiniteFloor : MonoBehaviour
{
    public Transform playerTransform;
    public static InfiniteFloor Instance;
    
    // Based on your Scale Z
    private float floorLength = 214.695f;

    void Update()
    {
        // Check if the player has moved beyond the current floor piece
        float halfLength = floorLength / 2f;
        // If the player has moved past the end of this floor piece
        if (playerTransform.position.z > transform.position.z + floorLength)
        {
            Vector3 newPos = transform.position;
            newPos.z += floorLength * 2; // Move it ahead by two lengths (since we have 2 pieces total)
            transform.position = newPos;
        }
        // If the player has moved before the start of this floor piece
        if (playerTransform.position.z < transform.position.z - floorLength)
        {
            Vector3 newPos = transform.position;
            newPos.z -= floorLength * 2;
            transform.position = newPos;
        }
    }
    // void Update()
    // {
    //     float buffer = 0.5f; // A small buffer to prevent visual gaps when the player moves fast
    //     // If the player has moved past the end of this floor piece
    //     if (playerTransform.position.z > transform.position.z + (floorLength / 2) + buffer)
    //     {
    //         // Move it ahead by two lengths (since we have 2 pieces total)
    //         Vector3 newPos = transform.position;
    //         newPos.z += floorLength * 2;
    //         transform.position = newPos;
    //     }
    //     else if (playerTransform.position.z < transform.position.z - (floorLength / 2) - buffer)
    //     {
    //         Vector3 newPos = transform.position;
    //         newPos.z -= floorLength * 2;
    //         transform.position = newPos;
    //     }
    // }
}