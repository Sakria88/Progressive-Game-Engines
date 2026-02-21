using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    // The prefab we want to spawn (the Coin)
    public GameObject CoinPrefab;
    public GameObject SpeedBooster;
    public Transform playerTransform;
    public static CollectiblesManager Instance; 
    
    private int coinCount = 0;
    // How many to spawn
    public int totalCollectibles = 10;
    public float spawnZOffset = 50f;
    
    public float spawnRangeX = 10f; // Range for random spawning
    public float distanceBetweenCoins = 20f;
    private float lastBoosterZ;// Distance between each coin
    // The area where they can spawn
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
    // Start is called before the first frame update
    void Start()
    {
       SpawnInitialCoins();
       SpawnSpeedBooster();

    }
    private void SpawnSpeedBooster()
    {
        float initialZ = playerTransform.position.z + spawnZOffset + 20f; // Start spawning boosters 20 units ahead of the player
        
        Vector3 spawnPos = new Vector3(0, 3f, initialZ); // Spawn the first booster
        
        Instantiate(SpeedBooster, spawnPos, Quaternion.identity);
        lastBoosterZ = initialZ; // Initialize the last booster position    
    }
    void SpawnInitialCoins()
    {
        for (int i = 0; i < totalCollectibles; i++)
        {
           float spawnZ = transform.position.z + spawnZOffset + (i * distanceBetweenCoins);

           SpawnCoinAtPosition(spawnZ);
        }
    }
    
    public void SpawnCoinAtPosition(float ZPos)
    {
        Vector3 randomPos = new Vector3(
            UnityEngine.Random.Range(-spawnRangeX, spawnRangeX), // Random Left/Right
            3f,                                    // Hover slightly off the floor
            ZPos
    );
    GameObject newCoin = Instantiate(CoinPrefab, randomPos, Quaternion.identity);
    newCoin.tag = "Coin"; // Ensure the coin has the correct tag for collision detection

    }
    
    public void AddCoins(int amount)
    {
        //update the coin count and log it
        coinCount += amount;
        Debug.Log("Total Coins: " + coinCount);
        float spawnFarAhead = playerTransform.position.z + (totalCollectibles * distanceBetweenCoins);
        SpawnCoinAtPosition(spawnFarAhead);

        // Every 5 coins, spawn a speed booster
        if (coinCount % 5 == 0) 
        {
            float nextBoosterZ = playerTransform.position.z + 30f; // Spawn a bit further ahead than the next coin
            Debug.Log("Spawning Booster at: " + nextBoosterZ);
            // Spawn a bit further ahead than the next coin to give the player time to react
            SpawnSpeedBoosterAt(nextBoosterZ); 
        }
    }

    private void SpawnSpeedBoosterAt(float zPos)
    {
        float initialZ = playerTransform.position.z + spawnZOffset + (totalCollectibles * distanceBetweenCoins) + 20f;
        // Default position if zPos is not greater than initialZ
        Vector3 spawnPos = new Vector3(0, 3f, initialZ); 
        GameObject booster = Instantiate(SpeedBooster, spawnPos, Quaternion.identity);
        booster.tag = "SpeedBooster"; // Important for your CollisionHandler!
        
        lastBoosterZ = initialZ; // Update the last booster position
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