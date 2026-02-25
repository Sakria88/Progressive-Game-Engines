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
    public float distanceBetweenCoins = 70f;
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
       AddSpeedBooster();

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
        if (CoinPrefab == null)
        {
            Debug.LogError("CoinPrefab is NULL or was destroyed. Assign a prefab asset from the Project window.");
            return;
        }

        Vector3 randomPos = new Vector3(
            UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
            3f,
            ZPos
        );

        GameObject newCoin = Instantiate(CoinPrefab, randomPos, Quaternion.identity);
        newCoin.tag = "Coin";

    }

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
    public void AddSpeedBooster()
    {
        if (coinCount % 5 == 0)
        {
            Debug.Log("Spawning Booster ");
            float initialZ = playerTransform.position.z + spawnZOffset + 20f;
            SpawnSpeedBoosterAt(initialZ);
            lastBoosterZ = initialZ;
        }
    }

   public void AddSpeedBoosterAt(float zPos)
   {
        SpawnSpeedBoosterAt(zPos);
        lastBoosterZ = zPos;
   }



    private void SpawnSpeedBoosterAt(float zPos)
    {
        if (SpeedBooster == null)
        {
            Debug.LogError("SpeedBooster is NULL or was destroyed. Assign a prefab asset from the Project window.");
            return;
        }

        Vector3 spawnPos = new Vector3(0, 3f, zPos);
        GameObject booster = Instantiate(SpeedBooster, spawnPos, Quaternion.identity);
        booster.tag = "SpeedBooster";
    }

  
}
//private void SpawnSpeedBooster()
//{
//    float initialZ = playerTransform.position.z + spawnZOffset + 20f; // Start spawning boosters 20 units ahead of the player

//    Vector3 spawnPos = new Vector3(0, 3f, initialZ); // Spawn the first booster

//    Instantiate(SpeedBooster, spawnPos, Quaternion.identity);
//    lastBoosterZ = initialZ; // Initialize the last booster position    
//}