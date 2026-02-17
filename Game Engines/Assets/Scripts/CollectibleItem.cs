using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
public class CollectibleItem : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
{
    // Check if we hit a collectible
    if (collision.gameObject.GetComponent<CollectibleItem>() != null)
    {
        myBackpack.AddToBackpack(); // Add to backpack
        Destroy(collision.gameObject); // Remove coin from world
        Debug.Log("Collected a " + collision.gameObject.name);
    }
    
    // Your existing Enemy check...
    if (collision.gameObject.CompareTag("Enemy"))
    {
        RespawnPlayer();
    }
}
}