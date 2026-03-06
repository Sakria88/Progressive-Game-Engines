// This script defines a collectible item that grants the 
//player a temporary speed boost when collected.
using UnityEngine;

public class SpeedBoosterCollectible : CollectibleBase
{
    [Header("Boost Tuning")]
    [Tooltip("1.0 = no boost. Try 1.25–1.6 for a controllable boost.")]
    [Range(1.0f, 2.0f)]
    [SerializeField] private float multiplier = 1.4f;

    [Tooltip("How long the boost lasts (seconds).")]
    [Range(0.5f, 15f)]
    [SerializeField] private float duration = 5f;

    [Header("Safety Clamp (prevents extreme speeds if multiplier is edited)")]
    [SerializeField] private float minMultiplier = 1.1f;
    [SerializeField] private float maxMultiplier = 2.1f;

    protected override bool OnCollected(PlayerCharacter player)
    {
         if (player == null) return false;
        Debug.Log("[Collectible] Speed Booster Collected");
        float safeMultiplier = Mathf.Clamp(multiplier, minMultiplier, maxMultiplier);
        return player.ActivateSpeedBoost(safeMultiplier, duration);
    }
}