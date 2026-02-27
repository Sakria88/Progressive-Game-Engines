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
    [SerializeField] private bool applyTagToChildren = false;

    private Transform npc2Container;
    private const string TagNPC2 = "NPC2";
    private int spawnedNPC2Count = 0;

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
        EnsureTagExists(TagNPC2);

        SpawnNPC2();

        if (verboseLogs)
        {
            Debug.Log($"[NPCManager:NPC2] Spawn complete. NPC2 spawned: {spawnedNPC2Count}");
        }
    }

    private void EnsureContainer()
    {
        GameObject c2 = GameObject.Find("NPC2_Container");
        if (c2 == null) c2 = new GameObject("NPC2_Container");
        npc2Container = c2.transform;
    }

    private void EnsureTagExists(string tagName)
    {
        if (!TagExists(tagName))
        {
            Debug.LogWarning($"[NPCManager:NPC2] Tag '{tagName}' does not exist. Create it in Unity Tags.");
        }
    }

    private bool TagExists(string tagName)
    {
        try
        {
            GameObject temp = new GameObject("TagTest");
            temp.tag = tagName;
            Destroy(temp);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void SpawnNPC2()
    {
        int spawned = 0;

        for (int i = 0; i < countPerType; i++)
        {
            GameObject selectedFloor = floorObjects[Random.Range(0, floorObjects.Length)];
            if (selectedFloor == null)
            {
                Debug.LogError("[NPCManager:NPC2] A floorObjects entry is NULL (Missing reference).");
                continue;
            }

            Renderer floorRenderer = selectedFloor.GetComponentInChildren<Renderer>();
            if (floorRenderer == null)
            {
                Debug.LogError($"[NPCManager:NPC2] No Renderer found on '{selectedFloor.name}' or its children.");
                continue;
            }

            float centerX = floorRenderer.bounds.center.x;
            float xPos = Random.Range(centerX - npc2LaneWidth, centerX + npc2LaneWidth);
            float surfaceY = floorRenderer.bounds.max.y;

            float zPos = playerTransform.position.z + spawnDistanceAhead + (i * zSpacing);
            Vector3 spawnPos = new Vector3(xPos, surfaceY + npc2YOffset, zPos);

            GameObject prefab = npcForwardPrefabs[i % npcForwardPrefabs.Count];
            if (prefab == null)
            {
                Debug.LogError("[NPCManager:NPC2] Prefab is missing in npcForwardPrefabs list.");
                continue;
            }

            GameObject npc = Instantiate(prefab, spawnPos, prefab.transform.rotation, npc2Container);
            npc.transform.localScale = prefab.transform.localScale;

            if (TagExists(TagNPC2))
            {
                npc.tag = TagNPC2;

                if (applyTagToChildren)
                {
                    Transform[] children = npc.GetComponentsInChildren<Transform>(true);
                    for (int c = 0; c < children.Length; c++)
                    {
                        children[c].gameObject.tag = TagNPC2;
                    }
                }
            }

            AddOrConfigureNPCComponent(npc, NPC.NPCMovementType.ForwardBackward);
            spawned++;

            if (verboseLogs && i == 0)
            {
                Debug.Log($"[NPCManager:NPC2] Example spawn: '{npc.name}' at {spawnPos} on floor '{selectedFloor.name}'");
            }
        }

        spawnedNPC2Count = spawned;
    }

    private void AddOrConfigureNPCComponent(GameObject npc, NPC.NPCMovementType type)
    {
        if (npc == null) return;

        NPC npcScript = npc.GetComponent<NPC>();
        if (npcScript == null)
        {
            npcScript = npc.AddComponent<NPC>();
        }

        npcScript.movementType = type;
        npcScript.moveSpeed = moveSpeed;
        npcScript.moveRange = moveRange;
        npcScript.waitTime = waitTime;
    }
}