// This script manages the spawning and collection of collectibles 
//in the game, including coins, speed boosters, and shields. 
//It handles the initial spawning of these items at set intervals 
//along the player's path and updates the coin count when coins are 
//collected. The manager also ensures that new collectibles are 
//spawned ahead of the player as they progress through the level.
using UnityEngine;
using DLLCollectables;

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
    [SerializeField] private int totalCollectibles = 400;
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

    [Header("Scoring")]
    [Tooltip("Every time total coins reaches a multiple of this, award points.")]
    [SerializeField] private int coinsPerScoreStep = 20;

    [Tooltip("Points to award per coin milestone.")]
    [SerializeField] private int pointsPerScoreStep = 100;


    public int coinCount{get; private set;}
    // DLL score system owned by the manager so ScoreUI can subscribe.
    private readonly ScoreSystem scoreSystem = new ScoreSystem();

    // Tracks the next milestone at which we should award points (20, 40, 60, ...)
    private int nextCoinMilestone;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        // Initialise scoring state
        scoreSystem.Reset();
        nextCoinMilestone = Mathf.Max(1, coinsPerScoreStep);
    }

    private void Start()
    {
        SpawnInitialBoosters();
        SpawnInitialShields();
        SpawnInitialCoins();
    }
    public ScoreSystem GetScoreSystem()
    {
        return scoreSystem;
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
        //Debug.Log($"[CollectiblesManager] {nameof(AddCoins)} id={GetInstanceID()} +{amount} coinCount(before)={coinCount}");
        if (amount <= 0) return false;
    
        // Update manager's internal coin count for scoring and milestone tracking
        coinCount += amount;
        Debug.Log("Total Coins: " + coinCount);
        // TotalCoinsCollected += amount;

        // Award score: +100 every time the player reaches 20 coins (and again at 40, 60, ...)
        if (coinsPerScoreStep > 0 && pointsPerScoreStep > 0)
        {
            while (coinCount >= nextCoinMilestone)
            {
                scoreSystem.AddPoints(pointsPerScoreStep);
                nextCoinMilestone += coinsPerScoreStep;
            }
        }

        if (playerTransform != null)
        {
            float spawnFarAhead = playerTransform.position.z + (totalCollectibles * distanceBetweenCoins);
            SpawnCoinAtPosition(spawnFarAhead);
        }
        return true;
    }

    public bool ResetCoins()
    {
        //Debug.Log($"[CollectiblesManager] {nameof(ResetCoins)} id={GetInstanceID()} coinCount(before)={coinCount}");
        coinCount = 0;

        // Reset milestone tracking (20, 40, 60...)
        nextCoinMilestone = coinsPerScoreStep;

        Debug.Log("Coins reset to 0");

        return true;
    }
    public bool ResetAllProgress()
    {
        coinCount = 0;
        nextCoinMilestone = coinsPerScoreStep;

        scoreSystem.Reset();

        Debug.Log("Coins and Score reset");

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