using UnityEngine;

public class Coin : MonoBehaviour
{
    private int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object touching the coin is the Player
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        // Tell the manager to add the value
        CollectiblesManager.Instance.AddCoins(value);

        // Optional: Play a sound effect or particle here
        
        Destroy(gameObject);
    }
}