using UnityEngine;

public class InfiniteFloor : MonoBehaviour
{
    public Transform playerTransform;
    // Based on your Scale Z
    private float floorLength = 214.695f; 

    void Update()
    {
        // If the player has moved past the end of this floor piece
        if (playerTransform.position.z > transform.position.z + (floorLength / 2))
        {
            // Move it ahead by two lengths (since we have 2 pieces total)
            Vector3 newPos = transform.position;
            newPos.z += floorLength * 2;
            transform.position = newPos;
        }
    }
}