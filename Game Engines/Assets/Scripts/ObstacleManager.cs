using System.Collections.Generic;
using UnityEngine;


public class ObstacleManager : MonoBehaviour
{
    
    public Transform player;
    public static ObstacleManager Instance;
    public GameObject shelfPrefab;
    public int shelfLimit = 40;
    
    
    public float spawnDistanceAhead = 150f; // How far ahead to spawn
    public float spawnRangeX = 100f;        // Floor width (Left/Right)
    public float floorYPosition = 3f;
    public float distanceBetweenObstacles = 150f;
    public float spawnZOffset = 30f; // Initial offset for spawning obstacles   
    public List<GameObject> obstacleList = new List<GameObject>();
    void Awake()
    {
        // Ensure only one manager exists
        if (Instance == null)
        {
            Instance = this;
        }
       
    }
    void Start()
    {
      
       SpawnInitialShelf();

    }

    
    void SpawnInitialShelf()
    {
        // Start spawning boosters 20 units ahead of the player
        for (int i = 0; i < shelfLimit; i++)
        {
            float zPos = player.position.z + spawnDistanceAhead + (i * distanceBetweenObstacles);
            SpawnNewObstacle(zPos);
        }
    }

    public void SpawnNewObstacle(float zPosition)
    {
        if (obstacleList.Count == 0)
        {
            Debug.LogError("No obstacles assigned to the ObstacleManager!");
            return;
        }

        // 1. Randomly select an obstacle from the list
        int index = Random.Range(0, obstacleList.Count);
        GameObject selectedPrefab = obstacleList[index];

        // 2. Calculate a random X position within the spawn range
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);

        // 3. Set the spawn position using the coin height logic
        Vector3 spawnPosition = new Vector3(randomX, floorYPosition, zPosition);

        // FIX 1: Use selectedPrefab.transform.rotation instead of Quaternion.identity
        // This ensures the shelf spawns standing up as shown in your first photo.
        GameObject newObstacle = Instantiate(selectedPrefab, spawnPosition, selectedPrefab.transform.rotation);

        // FIX 2: Pass 'newObstacle' (the clone) to SetupObstacle, NOT the prefab
        // This ensures the tag and components are added to the object in the scene.
        SetupObstacle(newObstacle);    
    }
    private void SetupObstacle(GameObject obj)
    {
        // Ensure the object has the "Obstacle" tag
        obj.tag = "Obstacle";
        // Ensure the object has an Obstacle component 
        if (obj.GetComponent<Obstacle>() == null)
        {
            obj.AddComponent<Obstacle>();
        }

    
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            // Adds a Rigidbody if none exists, so it can interact with physics
            rb = obj.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Make it static by default
            Debug.LogWarning(obj.name + " was missing a Rigidbody, one has been added and set to kinematic.");
        }
        if (obj.GetComponent<Collider>() == null)
        {
            // Adds a BoxCollider by default if none exists
            obj.AddComponent<BoxCollider>();
            Debug.LogWarning(obj.name + " was missing a collider, one has been added.");
        }
    }
}