// This manager handles the spawning of NPC1 (side-to-side movement) on the floor.
using System.Collections.Generic;
using UnityEngine;

public class NPC1Manager : MonoBehaviour
{
    [Header("Lane Settings (NPC1)")]
    [SerializeField] private int laneCount = 3;

    [Tooltip("Manual lane distance between lanes (used if Auto Fit Lanes To Floor is OFF).")]
    [SerializeField] private float laneStep = 2.5f;

    [Tooltip("Manual max horizontal distance from floor center NPC1 is allowed to spawn (used if Auto Fit is OFF).")]
    [SerializeField] private float npc1LaneWidth = 3f;

    [Tooltip("If ON, laneStep and npc1LaneWidth are calculated from the selected floor's bounds every spawn.")]
    [SerializeField] private bool autoFitLanesToFloor = true;

    [Tooltip("Padding from each edge of the floor to avoid spawning too close to the rim.")]
    [SerializeField] private float edgePadding = 1.0f;

    [Header("Side-to-Side NPC Prefabs (NPC1)")]
    [SerializeField] private List<GameObject> npc1Prefabs = new List<GameObject>();

    [Header("Floor Detection")]
    [SerializeField] private GameObject[] floorObjects;

    [Header("Player Reference")]
    [SerializeField] private Transform playerTransform;

    [Header("Spawn Settings")]
    [SerializeField] private int countPerType = 15;
    [SerializeField] private float spawnDistanceAhead = 50f;
    [SerializeField] private float zSpacing = 160f;

    [Header("NPC1 Position Settings")]
    [SerializeField] private float npc1YOffset = 0f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveRange = 7f;
    [SerializeField] private float waitTime = 0f;

    [Header("Debug")]
    [SerializeField] private bool verboseLogs = true;
    [SerializeField] private bool applyTagToChildren = false;

    private Transform npc1Container;
    private const string TagNPC1 = "NPC1";
    private int spawnedNPC1Count = 0;

    private void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("[NPC1Manager] Player Transform not assigned.");
            return;
        }

        if (floorObjects == null || floorObjects.Length == 0)
        {
            Debug.LogError("[NPC1Manager] floorObjects is empty.");
            return;
        }

        if (npc1Prefabs == null || npc1Prefabs.Count == 0)
        {
            Debug.LogWarning("[NPC1Manager] npc1Prefabs is empty.");
            return;
        }

        laneCount = Mathf.Max(1, laneCount);
        edgePadding = Mathf.Max(0f, edgePadding);

        EnsureContainer();
        SpawnNPC1();

        if (verboseLogs)
        {
            Debug.Log($"[NPC1Manager] Spawn complete. NPC1 spawned: {spawnedNPC1Count}");
        }
    }

    private void EnsureContainer()
    {
        GameObject c1 = GameObject.Find("NPC1_Container");
        if (c1 == null) c1 = new GameObject("NPC1_Container");
        npc1Container = c1.transform;
    }

    private void SpawnNPC1()
    {
        int spawned = 0;

        for (int i = 0; i < countPerType; i++)
        {
            GameObject selectedFloor = floorObjects[Random.Range(0, floorObjects.Length)];
            if (selectedFloor == null)
            {
                if (verboseLogs) Debug.LogWarning("[NPC1Manager] Selected floor was null.");
                continue;
            }

            Renderer floorRenderer = selectedFloor.GetComponentInChildren<Renderer>();
            if (floorRenderer == null)
            {
                Debug.LogError($"[NPC1Manager] No Renderer found on '{selectedFloor.name}' or its children.");
                continue;
            }

            Bounds bounds = floorRenderer.bounds;

            // Compute safe X range inside the floor
            float minX = bounds.min.x + edgePadding;
            float maxX = bounds.max.x - edgePadding;

            // If padding is too big, fall back to full bounds to avoid invalid range
            if (maxX <= minX)
            {
                minX = bounds.min.x;
                maxX = bounds.max.x;
            }

            float floorWidthX = maxX - minX;
            float centerX = (minX + maxX) * 0.5f;

            if (verboseLogs && i == 0)
            {
                Debug.Log($"[NPC1Manager] Floor '{selectedFloor.name}' widthX={bounds.size.x:F2} safeWidthX={floorWidthX:F2} centerX={bounds.center.x:F2}");
            }

            // AUTO FIT: laneStep and npc1LaneWidth derived from floor width
            float stepUsed = laneStep;
            float laneWidthUsed = npc1LaneWidth;

            if (autoFitLanesToFloor)
            {
                if (laneCount <= 1)
                {
                    stepUsed = 0f;
                    laneWidthUsed = 0f;
                }
                else
                {
                    // Evenly distribute lanes across safe width
                    stepUsed = floorWidthX / (laneCount - 1);

                    // Max lane offset from center to reach edge lanes
                    laneWidthUsed = (floorWidthX * 0.5f);
                }
            }

            // --- Choose lane index and compute xPos on the safe range ---
            int laneIndex = Random.Range(0, laneCount);

            float xPos;
            if (laneCount <= 1)
            {
                xPos = centerX;
            }
            else
            {
                // Lanes go from minX to maxX
                float t = laneIndex / (float)(laneCount - 1); // 0..1
                xPos = Mathf.Lerp(minX, maxX, t);
            }

            // Additional clamp: ensure within lane width relative to center (manual or auto)
            float clampedOffset = Mathf.Clamp(xPos - centerX, -laneWidthUsed, laneWidthUsed);
            xPos = centerX + clampedOffset;

            // Final safety clamp to safe bounds
            xPos = Mathf.Clamp(xPos, minX, maxX);

            float surfaceY = bounds.max.y;

            float zPos = playerTransform.position.z
                         + spawnDistanceAhead
                         + (i * zSpacing)
                         + (zSpacing * 0.5f);

            Vector3 spawnPos = new Vector3(xPos, surfaceY + npc1YOffset, zPos);

            GameObject prefab = npc1Prefabs[i % npc1Prefabs.Count];
            if (prefab == null)
            {
                Debug.LogError("[NPC1Manager] Prefab missing in npc1Prefabs list.");
                continue;
            }

            GameObject npc = Instantiate(prefab, spawnPos, prefab.transform.rotation, npc1Container);
            npc.transform.localScale = prefab.transform.localScale;

            // Tag assignment
            npc.tag = TagNPC1;

            if (applyTagToChildren)
            {
                foreach (Transform child in npc.GetComponentsInChildren<Transform>(true))
                {
                    child.gameObject.tag = TagNPC1;
                }
            }

            // Configure NPC and IMPORTANT: lock side-to-side limits to this floor
            NPC npcScript = npc.GetComponent<NPC>();
            if (npcScript == null)
            {
                npcScript = npc.AddComponent<NPC>();
            }

            npcScript.movementType = NPC.NPCMovementType.SideToSide;
            npcScript.moveSpeed = moveSpeed;
            npcScript.moveRange = moveRange;
            npcScript.waitTime = waitTime;

            // NEW: prevents walking/spawning off the floor
            npcScript.SetSideToSideLimits(minX, maxX);

            spawned++;
        }

        spawnedNPC1Count = spawned;
    }
}