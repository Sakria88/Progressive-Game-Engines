using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
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
            PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();

            if (playerCharacter != null)
            {
                // Activate the speed boost on the player character
                playerCharacter.ActivateSpeedBoost(2.0f, 5.0f); 
                Debug.Log("SpeedBooster: Activated speed boost for player ");
                // Start the boost on the PLAYER's script so it doesn't die
                // StartCoroutine(ApplySpeedBoost(playerCharacter));
            }
            GetComponent<MeshRenderer>().enabled = false; // Hide the speed booster visually
            GetComponent<Collider>().enabled = false; // Disable the collider to prevent multiple triggers
         
            
        }
    }

    private IEnumerator ApplySpeedBoost(PlayerCharacter playerCharacter)
    {
        playerCharacter.moveSpeed += speedIncrease; // Increase the player's speed
        yield return new WaitForSeconds(boostDuration); // Wait for the boost duration
        playerCharacter.moveSpeed -= speedIncrease; // Reset the player's speed
        Destroy(gameObject); // Destroy the speed booster object after the boost duration
    }
}