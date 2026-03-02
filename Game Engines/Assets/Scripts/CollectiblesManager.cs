// This script manages the spawning and collection of collectibles 
//in the game, including coins, speed boosters, and shields. 
//It handles the initial spawning of these items at set intervals 
//along the player's path and updates the coin count when coins are 
//collected. The manager also ensures that new collectibles are 
//spawned ahead of the player as they progress through the level.
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject CoinPrefab;
    [SerializeField] private GameObject SpeedBooster;
    [SerializeField] private GameObject ShieldPrefab;

    [Header("References")]
    [SerializeField] private Transform playerTransform;

    public static CollectiblesManager Instance { get; private set; }

    [Header("Coins")]
    [SerializeField] private int totalCollectibles = 10;
    [SerializeField] private float spawnZOffset = 50f;
    [SerializeField] private float spawnRangeX = 10f;
    [SerializeField] private float distanceBetweenCoins = 70f;

    [Header("Boosters")]
    [SerializeField] private int totalBoosters = 25;
    [SerializeField] private float boosterSpawnRangeX = 10f;
    [SerializeField] private float boosterDistance = 200f;

    [Header("Shields")] 
    [SerializeField] private int totalShields = 10;
    [SerializeField] private float shieldSpawnRangeX = 10f;
    [SerializeField] private float shieldDistance = 300f;


    private int coinCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SpawnInitialBoosters();
        SpawnInitialShields();
        SpawnInitialCoins();
    }

    private bool SpawnInitialCoins()
    {
        for (int i = 0; i < totalCollectibles; i++)
        {
            float spawnZ = transform.position.z + spawnZOffset + (i * distanceBetweenCoins);
            SpawnCoinAtPosition(spawnZ);
        }
        return true;
    }

    public bool SpawnCoinAtPosition(float zPos)
    {
        if (CoinPrefab == null) return false;

        Vector3 randomPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 3f, zPos);
        Instantiate(CoinPrefab, randomPos, Quaternion.identity);
        return true;
    }

    public bool AddCoins(int amount)
    {
        coinCount += amount;
        Debug.Log("Total Coins: " + coinCount);

        if (playerTransform != null)
        {
            float spawnFarAhead = playerTransform.position.z + (totalCollectibles * distanceBetweenCoins);
            SpawnCoinAtPosition(spawnFarAhead);
        }
        return true;
    }

    private bool SpawnInitialBoosters()
    {
        if (SpeedBooster == null || playerTransform == null) return false;

        for (int i = 0; i < totalBoosters; i++)
        {
            float spawnZ = playerTransform.position.z + spawnZOffset + (i * boosterDistance);
            float spawnX = Random.Range(-boosterSpawnRangeX, boosterSpawnRangeX);

            Vector3 spawnPos = new Vector3(spawnX, 3f, spawnZ);
            Instantiate(SpeedBooster, spawnPos, Quaternion.identity);
        }

        return true;
    }

    private bool SpawnInitialShields()
    {
        if (ShieldPrefab == null || playerTransform == null) return false;

        for (int i = 0; i < totalShields; i++)
        {
            float spawnZ = playerTransform.position.z + spawnZOffset + (i * shieldDistance);
            float spawnX = Random.Range(-shieldSpawnRangeX, shieldSpawnRangeX);

            Vector3 spawnPos = new Vector3(spawnX, 3f, spawnZ);
            Instantiate(ShieldPrefab, spawnPos, Quaternion.identity);
        }

        return true;
    }
}