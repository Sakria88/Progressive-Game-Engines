// This script manages the spawning of NPC2 (forward-backward movement) 
//on the floor.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This manager handles the spawning of NPC2 (forward-backward movement) on the floor.
public class NPCManager : MonoBehaviour
{
    [Header("Forward-Backward NPC Prefabs (NPC2)")]
    [SerializeField] private List<GameObject> npcForwardPrefabs = new List<GameObject>();

    [Header("Floor Detection (Objects that represent your ground)")]
    [SerializeField] private GameObject[] floorObjects;

    [Header("Player Reference")]
    [SerializeField] private Transform playerTransform;

    [Header("Spawn Settings")]
    [SerializeField] private int countPerType = 15;
    [SerializeField] private float spawnDistanceAhead = 50f;
    [SerializeField] private float zSpacing = 160f;

    [Header("NPC2 Position Settings")]
    [SerializeField] private float npc2LaneWidth = 8f;
    [SerializeField] private float npc2YOffset = 1.2f;

    [Header("Movement Settings (applied to spawned NPC2)")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveRange = 7f;
    [SerializeField] private float waitTime = 0f;

    [Header("Debug")]
    [SerializeField] private bool verboseLogs = true;

    private Transform npc2Container;

    private int spawnedNPC2Count;

    private void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("[NPCManager:NPC2] Player Transform not assigned.");
            return;
        }

        if (floorObjects == null || floorObjects.Length == 0)
        {
            Debug.LogError("[NPCManager:NPC2] floorObjects is empty. Assign your Ground objects in the Inspector.");
            return;
        }

        if (npcForwardPrefabs == null || npcForwardPrefabs.Count == 0)
        {
            Debug.LogWarning("[NPCManager:NPC2] npcForwardPrefabs is empty (NPC2 will not spawn).");
            return;
        }

        EnsureContainer();
        SpawnNPC2();

        if (verboseLogs)
        {
            Debug.Log($"[NPCManager:NPC2] Spawn complete. NPC2 spawned: {spawnedNPC2Count}");
        }
    }

    private bool EnsureContainer()
    {
        GameObject c2 = GameObject.Find("NPC2_Container");
        if (c2 == null) c2 = new GameObject("NPC2_Container");
        npc2Container = c2.transform;
        return true;
    }

    private bool SpawnNPC2()
    {
        int spawned = 0;

        for (int i = 0; i < countPerType; i++)
        {
            GameObject selectedFloor = floorObjects[Random.Range(0, floorObjects.Length)];
            if (selectedFloor == null) continue;

            Renderer floorRenderer = selectedFloor.GetComponentInChildren<Renderer>();
            if (floorRenderer == null) continue;

            float centerX = floorRenderer.bounds.center.x;
            float xPos = Random.Range(centerX - npc2LaneWidth, centerX + npc2LaneWidth);
            float surfaceY = floorRenderer.bounds.max.y;

            float zPos = playerTransform.position.z + spawnDistanceAhead + (i * zSpacing);
            Vector3 spawnPos = new Vector3(xPos, surfaceY + npc2YOffset, zPos);

            GameObject prefab = npcForwardPrefabs[i % npcForwardPrefabs.Count];
            if (prefab == null) continue;

            GameObject npc = Instantiate(prefab, spawnPos, prefab.transform.rotation, npc2Container);
            npc.transform.localScale = prefab.transform.localScale;

            ConfigureNPC(npc, NPCCharacter.NPCMovementType.ForwardBackward);
            spawned++;
        }

        spawnedNPC2Count = spawned;
        return true;
    }

    private bool ConfigureNPC(GameObject npc, NPCCharacter.NPCMovementType type)
    {
        if (npc == null) return false;

        NPCCharacter npcScript = npc.GetComponent<NPCCharacter>();
        if (npcScript == null)
        {
            npcScript = npc.AddComponent<NPCCharacter>();
        }

        npcScript.movementType = type;
        npcScript.moveSpeed = moveSpeed;
        npcScript.moveRange = moveRange;
        npcScript.waitTime = waitTime;
        return true;
    }
}