using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    // The prefab we want to spawn (the Coin)
    public GameObject CoinPrefab;
    public static CollectiblesManager Instance; 
    
    private int coinCount = 0;
    // How many to spawn
    public int totalCollectibles = 10;
    
    // The area where they can spawn
    public Vector3 spawnAreaSize = new Vector3(20, 0, 20);
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCollectiblesOverTime());
    }

    IEnumerator SpawnCollectiblesOverTime()
    {
        for (int i = 0; i < totalCollectibles; i++)
        {
            // Generate a random position
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                1.0f, // Height off the ground
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            // Spawn the object
            Instantiate(CoinPrefab, randomPos, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f); // Wait a bit before spawning the next batch (optional)
    }
    private void Awake()
    {
        // Ensure only one manager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        //update the coin count and log it
        coinCount += amount;
        Debug.Log("Total Coins: " + coinCount);
    }

    public bool TrySpendCoins(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            Debug.Log("Spent " + amount + " coins. Remaining: " + coinCount);
            return true;
        }
        else
        {
            Debug.Log("Not enough coins to spend! Current: " + coinCount);
            return false;
        }
    }
}