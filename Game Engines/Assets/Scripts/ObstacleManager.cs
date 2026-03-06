// This script manages the spawning of 
//obstacles (shelves and computer desks) on the floor.
using System.Collections.Generic;
using UnityEngine;


public class ObstacleManager : MonoBehaviour
{
    
    public Transform player;
    public static ObstacleManager Instance;
    public GameObject shelfPrefab;
    public GameObject CompDeskPrefab;
    public int shelfLimit = 40;
    public int compdeskLimit = 40;
    
    
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
    //    SpawnInitialCompDesk();

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

        // Randomly select an obstacle from the list
        int index = Random.Range(0, obstacleList.Count);
        GameObject selectedPrefab = obstacleList[index];

        // Calculate a random X position within the spawn range
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);

        // Get the Y position from the floor's bounds
        float currentY = floorYPosition;
        
        // Adjust height based on the name of the prefab
        if (selectedPrefab.name.Contains("Shelf"))
        {
            currentY = 3.0f; //shelf height
        }
        else if (selectedPrefab.name.Contains("CompDesk"))
        {
            currentY = 0.837f; //desk height
        }
        // Set the spawn position using the coin height logic
        Vector3 spawnPosition = new Vector3(randomX, currentY, zPosition);

        // changed to using selectedPrefab.transform.rotation instead of Quaternion.identity
        // This ensures the shelf spawns stands like the original.
        GameObject newObstacle = Instantiate(selectedPrefab, spawnPosition, selectedPrefab.transform.rotation);

        //Pass 'newObstacle' (the clone) to SetupObstacle 
        // instead of 'selectedPrefab' (the original prefab)
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
            // Add a Rigidbody if none exists, so it can interact with physics
            rb = obj.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Make it static by default
            Debug.LogWarning(obj.name + " was missing a Rigidbody, one has been added and set to kinematic.");
        }
        if (obj.GetComponent<Collider>() == null)
        {
            // Add a BoxCollider by default if none exists
            obj.AddComponent<BoxCollider>();
            Debug.LogWarning(obj.name + " was missing a collider, one has been added.");
        }
    }
}