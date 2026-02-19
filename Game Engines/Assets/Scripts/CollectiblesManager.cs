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
    public float distanceBetweenCoins = 20f; // Distance between each coin
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
        Vector3 spawnPos = new Vector3(0, 3f, playerTransform.position.z + spawnZOffset + (totalCollectibles * distanceBetweenCoins) + 30f);
        Instantiate(SpeedBooster, spawnPos, Quaternion.identity);
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
    //IEnumerator SpawnCollectiblesOverTime()
    //{
        //for (int i = 0; i < totalCollectibles; i++)
        //{
            // Generate a random position
           //Vector3 randomPos = new Vector3(
               // Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                //1.0f, // Height off the ground
                //Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
           // );

            // Spawn the object
           // Instantiate(CoinPrefab, randomPos, Quaternion.identity);
            // Wait a bit before spawning the next batch to avoid overwhelming the player
            //yield return new WaitForSeconds(0.5f);
        //}
        
    //}
    public void AddCoins(int amount)
    {
        //update the coin count and log it
        coinCount += amount;
        Debug.Log("Total Coins: " + coinCount);
        float spawnFarAhead = playerTransform.position.z + (totalCollectibles * distanceBetweenCoins);
        SpawnCoinAtPosition(spawnFarAhead);
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