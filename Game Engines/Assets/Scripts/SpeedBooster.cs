using System.Collections;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    public float speedIncrease = 20f; // Amount to increase the player's speed
    public float boostDuration = 3f; // Duration of the speed boost in seconds

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!collected &&other.CompareTag("Player"))
        {
            collected = true; // Ensure the boost is only applied once
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                // Start the boost on the PLAYER's script so it doesn't die
                StartCoroutine(ApplySpeedBoost(playerController));
            }
            GetComponent<MeshRenderer>().enabled = false; // Hide the speed booster visually
            GetComponent<Collider>().enabled = false; // Disable the collider to prevent multiple triggers
         
            
        }
    }

    private IEnumerator ApplySpeedBoost(PlayerController playerController)
    {
        playerController.moveSpeed += speedIncrease; // Increase the player's speed
        yield return new WaitForSeconds(boostDuration); // Wait for the boost duration
        playerController.moveSpeed -= speedIncrease; // Reset the player's speed
        Destroy(gameObject); // Destroy the speed booster object after the boost duration
    }
}