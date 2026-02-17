using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    // The prefab we want to spawn (the Coin)
    public GameObject CoinPrefab;
    
    // How many to spawn
    public int totalCollectibles = 10;
    
    // The area where they can spawn
    public Vector3 spawnAreaSize = new Vector3(20, 0, 20);
    // Start is called before the first frame update
    void Start()
    {
        SpawnCollectibles();
    }

    void SpawnCollectibles()
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
    }
}