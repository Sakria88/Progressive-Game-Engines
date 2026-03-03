// This script manages the spawning of NPC2 (forward-backward movement) 
//on the floor.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This manager now handles Side-to-Side movement for NPCs.
//and should for the future handle forward and backward.
public class NPCManager : MonoBehaviour
{
    [Header("Lane Settings (NPC2 and NPC1)")]
    [SerializeField] private int laneCount = 3;

    [Header("NPC Prefabs")]
    [SerializeField] private List<GameObject> npcPrefabs = new List<GameObject>();

    [Header("Floor Detection")]
    [SerializeField] private GameObject[] floorObjects;

    [Header("Player Reference")]
    [SerializeField] private Transform playerTransform;

    [Header("Spawn Settings")]
    [SerializeField] private int countPerType = 15;
    [SerializeField] private float spawnDistanceAhead = 50f;
    [SerializeField] private float zSpacing = 160f;

    [Header("NPC Position Settings")]
    [SerializeField] private float npcLaneWidth = 8f;
    [SerializeField] private float npcYOffset = 1.2f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveRange = 7f;
    [SerializeField] private float waitTime = 0f;

    [Header("Debug")]
    [SerializeField] private bool verboseLogs = true;

    private Transform npcContainer;
    private int spawnedNPCCount;

    private void Start()
    {
        if (playerTransform == null || floorObjects == null || floorObjects.Length == 0) return;

        EnsureContainer();
        SpawnNPCs();
    }

    private void EnsureContainer()
    {
        GameObject container = GameObject.Find("NPC_SideToSide_Container");
        if (container == null) container = new GameObject("NPC_SideToSide_Container");
        npcContainer = container.transform;
    }

    private void SpawnNPCs()
    {
        int spawned = 0;

        for (int i = 0; i < countPerType; i++)
        {
            GameObject selectedFloor = floorObjects[Random.Range(0, floorObjects.Length)];
            if (selectedFloor == null) continue;

            Renderer floorRenderer = selectedFloor.GetComponentInChildren<Renderer>();
            if (floorRenderer == null) continue;

            // Calculate floor boundaries for clamping
            float centerX = floorRenderer.bounds.center.x;
            float minX = floorRenderer.bounds.min.x;
            float maxX = floorRenderer.bounds.max.x;

            float xPos = Random.Range(centerX - npcLaneWidth, centerX + npcLaneWidth);
            float surfaceY = floorRenderer.bounds.max.y;
            float zPos = playerTransform.position.z + spawnDistanceAhead + (i * zSpacing);

            Vector3 spawnPos = new Vector3(xPos, surfaceY + npcYOffset, zPos);

            // Fix: Changed from 'npcSideToSide' to 'npcPrefabs'
            GameObject prefab = npcPrefabs[i % npcPrefabs.Count];
            if (prefab == null) continue;

            GameObject npc = Instantiate(prefab, spawnPos, prefab.transform.rotation, npcContainer);
            
            // Set up the script and apply SideToSide logic
            ConfigureNPC(npc, NPCCharacter.NPCMovementType.SideToSide, minX, maxX);
            spawned++;
        }

        spawnedNPCCount = spawned;
    }

    private void ConfigureNPC(GameObject npc, NPCCharacter.NPCMovementType type, float minX, float maxX)
    {
        NPCCharacter npcScript = npc.GetComponent<NPCCharacter>();
        if (npcScript == null) npcScript = npc.AddComponent<NPCCharacter>();

        npcScript.movementType = type;
        npcScript.moveSpeed = moveSpeed;
        npcScript.moveRange = moveRange;
        npcScript.waitTime = waitTime;

        // Pass the floor limits to the new NPCCharacter script
        npcScript.SetSideToSideLimits(minX, maxX);
    }
}