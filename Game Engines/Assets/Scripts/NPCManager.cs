// This script manages the spawning of NPC2 (forward-backward movement) 
//on the floor.
using System.Collections.Generic;
using UnityEngine;
using DLLEnemy;

public class NPCManager : MonoBehaviour
{
    [Header("Lane Settings")]
    [SerializeField] private int laneCount = 3;

    [Header("NPC Prefabs")]
    [SerializeField] private List<GameObject> npcPrefabs = new List<GameObject>();

    [Header("Floor Detection")]
    [SerializeField] private GameObject[] floorObjects;

    [Header("Player Reference")]
    [SerializeField] private Transform playerTransform;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistanceAhead = 50f;
    [SerializeField] private float zSpacing = 160f;
    [SerializeField] private int totalNPCs = 50;
    [SerializeField] private int sideToSideCount = 25;
    [SerializeField] private int forwardBackCount = 25;

    [Header("NPC Position Settings")]
    [SerializeField] private float npcLaneWidth = 8f;
    [SerializeField] private float npcYOffset = 1.2f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveRange = 7f;
    [SerializeField] private float waitTime = 0f;

    [Header("Debug")]
    [SerializeField] private bool verboseLogs = false;

    private List<GameObject> activeNPCs = new List<GameObject>();
    private Transform npcContainer;
    private int spawnedNPCCount;

    private void Start()
    {
        if (playerTransform == null || floorObjects == null || floorObjects.Length == 0 || npcPrefabs.Count == 0)
        {
            Debug.LogError("NPCManager: Missing references!");
            return;
        }

        Debug.Log($"NPC Prefabs: {npcPrefabs.Count}");
        Debug.Log($"Floor Objects: {floorObjects.Length}");

        EnsureContainer();
        SpawnAllNPCs();
    }

    private void Update()
    {
        for (int i = 0; i < activeNPCs.Count; i++)
        {
            GameObject npc = activeNPCs[i];
            if (npc == null) continue;

            // Respawn if far behind player
            if (npc.transform.position.z < playerTransform.position.z - 10f)
            {
                RespawnNPC(npc);
            }
        }
    }

    private void EnsureContainer()
    {
        GameObject container = GameObject.Find("NPC_SideToSide_Container");
        if (container == null)
        {
            container = new GameObject("NPC_SideToSide_Container");
        }
        npcContainer = container.transform;
    }

    private void SpawnAllNPCs()
    {
        activeNPCs.Clear();

        int spawned = 0;

        for (int i = 0; i < totalNPCs; i++)
        {
            NPCCharacter.NPCMovementType type = (i < forwardBackCount)
                ? NPCCharacter.NPCMovementType.ForwardBackward
                : NPCCharacter.NPCMovementType.SideToSide;

            SpawnSingleNPC(type, i);
            spawned++;
        }

        spawnedNPCCount = spawned;

        if (verboseLogs)
        {
            Debug.Log($"Spawned {spawned} NPCs (ForwardBack: {forwardBackCount}, Side: {totalNPCs - forwardBackCount})");
        }
    }
    private void SpawnSingleNPC(NPCCharacter.NPCMovementType type, int index)
    {
        Debug.Log($"SpawnSingleNPC called: type={type}, index={index}");

        if (npcPrefabs == null || npcPrefabs.Count == 0)
        {
            Debug.LogError("NPC Prefabs list is empty!");
            return;
        }

        if (floorObjects == null || floorObjects.Length == 0)
        {
            Debug.LogError("Floor objects array is empty!");
            return;
        }

        GameObject selectedFloor = floorObjects[Random.Range(0, floorObjects.Length)];
        if (selectedFloor == null)
        {
            Debug.LogError("Selected floor is null!");
            return;
        }

        Renderer floorRenderer = selectedFloor.GetComponentInChildren<Renderer>();
        if (floorRenderer == null)
        {
            Debug.LogError($"No Renderer on floor: {selectedFloor.name}");
            return;
        }

        float xPos = Random.Range(floorRenderer.bounds.min.x, floorRenderer.bounds.max.x);
        float surfaceY = floorRenderer.bounds.max.y;
        float zPos = playerTransform.position.z + spawnDistanceAhead + (index * zSpacing * 2f);

        Vector3 spawnPos = new Vector3(xPos, surfaceY + npcYOffset, zPos);

        GameObject prefab = npcPrefabs[index % npcPrefabs.Count];
        if (prefab == null)
        {
            Debug.LogError($"Prefab at index {index} is null!");
            return;
        }

        Debug.Log($"Instantiating prefab: {prefab.name} at {spawnPos}");

        GameObject npc = Instantiate(prefab, spawnPos, Quaternion.identity, npcContainer);

        if (npc == null)
        {
            Debug.LogError("Instantiate returned null!");
            return;
        }

        Debug.Log($"Spawned NPC: {npc.name}");

        activeNPCs.Add(npc);
        ConfigureNPC(npc, type, floorRenderer.bounds.min.x, floorRenderer.bounds.max.x);
    }
   

    private void ConfigureNPC(GameObject npc, NPCCharacter.NPCMovementType type, float minX, float maxX)
    {
        NPCCharacter npcScript = npc.GetComponent<NPCCharacter>();
        if (npcScript == null)
        {
            npcScript = npc.AddComponent<NPCCharacter>();
        }

        npcScript.movementType = type;
        npcScript.moveSpeed = moveSpeed;
        npcScript.moveRange = moveRange;
        npcScript.waitTime = waitTime;

        npcScript.SetSideToSideLimits(minX, maxX);
    }

    private void RespawnNPC(GameObject npc)
    {
        GameObject selectedFloor = floorObjects[Random.Range(0, floorObjects.Length)];
        if (selectedFloor == null) return;

        Renderer floorRenderer = selectedFloor.GetComponentInChildren<Renderer>();
        if (floorRenderer == null) return;

        float centerX = floorRenderer.bounds.center.x;
        float minX = floorRenderer.bounds.min.x;
        float maxX = floorRenderer.bounds.max.x;

        float xPos = Random.Range(centerX - npcLaneWidth, centerX + npcLaneWidth);
        float surfaceY = floorRenderer.bounds.max.y;

        float zPos = playerTransform.position.z + spawnDistanceAhead;
        zPos += Random.Range(0f, zSpacing * 0.5f);

        npc.transform.position = new Vector3(xPos, surfaceY + npcYOffset, zPos);

        ConfigureNPC(npc, NPCCharacter.NPCMovementType.SideToSide, minX, maxX);
    }
}



 