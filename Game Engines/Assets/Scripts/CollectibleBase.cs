// This is the base class for all collectibles in the game. 
//It handles the trigger detection and calls the abstract 
//OnCollected method which derived classes must implement 
//to define their specific collection behavior 
//(e.g. increasing score, granting power-ups, etc.). 
//The collectible will be destroyed upon successful collection.
using UnityEngine;
using DLLCollectables;

[RequireComponent(typeof(Collider))]
public abstract class CollectibleBase : MonoBehaviour
{
    protected virtual void Reset()
    {
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a PlayerCharacter
        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
        if (player == null)
        {
            Debug.Log($"[CollectibleBase] Trigger hit by {other.name} but no PlayerCharacter found in parent.");
            return;
        }

        Debug.Log($"[CollectibleBase] Collected by {player.name}");

        bool collected = OnCollected(player);
        if (collected)
        {
            Destroy(gameObject);
        }
    }

    protected abstract bool OnCollected(PlayerCharacter player);
}