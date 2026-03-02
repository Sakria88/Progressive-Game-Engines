// This script defines a coin collectible that the player 
//can pick up to increase their coin count. It interacts with the 
//CollectiblesManager to update the total coins 
//collected and also updates the player's coin count for UI purposes.
using UnityEngine;

public class CoinCollectible : CollectibleBase
{
    [SerializeField] private int amount = 1;

    protected override bool OnCollected(PlayerCharacter player)
    {
        if (player == null) return false;

        if (CollectiblesManager.Instance != null)
        {
            CollectiblesManager.Instance.AddCoins(amount);
        }

        return player.AddCoin(amount);   // Also update the player's coin count (for UI, etc.)
    }
}