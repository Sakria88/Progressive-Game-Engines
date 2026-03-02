// This script defines a shield collectible that the 
//player can pick up to activate a temporary shield.
using UnityEngine;

public class ShieldCollectible : CollectibleBase
{
    [SerializeField] private float durationSeconds = 5f;

    protected override bool OnCollected(PlayerCharacter player)
    {
        if (player == null) return false;
        Debug.Log("[Collectible] Shield Collected");
        return player.ActivateShield(durationSeconds);
    }
}