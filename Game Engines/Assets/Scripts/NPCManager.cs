using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    
    [Header("Forward-Backward NPCs")]
    public List<GameObject> NPCForward;

    [Header("Side-to-Side NPCs")]
    public List<GameObject> NPCSide;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float moveRange = 5f;
    public float waitTime = 1f;

    [Header("NPC count")]
    public int numNPCs = 10;           // Number per type
    public float zSpacing = 300f;      // Distance between each NPC
    public float laneWidth = 10f;      // Max horizontal deviation for side-to-side NPCs
    public float forwardRange = 5f;


    public Transform playerTransform;


    private void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform not assigned in NPCManager!");
            return;
        }
        SpawnNPCs();

        // Assign movement to forward-backward NPCs
        foreach (GameObject npc in NPCForward)
        {
            AddNPCComponent(npc, NPC.NPCMovementType.ForwardBackward);
        }

        // Assign movement to side-to-side NPCs
        foreach (GameObject npc in NPCSide)
        {
            AddNPCComponent(npc, NPC.NPCMovementType.SideToSide);
        }
    }

    private void SpawnNPCs()
    {
        float startZ = playerTransform.position.z + 50f; // spawn ahead of player

        // --- Forward-Backward NPCs ---
        for (int i = 0; i < numNPCs; i++)
        {
            // Random X position within lane width so NPC stays on floor
            float xPos = Random.Range(-laneWidth, laneWidth);
            Vector3 pos = new Vector3(xPos, 0, startZ + i * zSpacing);

            GameObject prefab = NPCForward[i % NPCForward.Count];

            // Instantiate with prefab's original rotation and scale
            GameObject npc = Instantiate(prefab, pos, prefab.transform.rotation);
            npc.transform.localScale = prefab.transform.localScale;

            npc.tag = "NPC";
            AddNPCComponent(npc, NPC.NPCMovementType.ForwardBackward);
        }

        // --- Side-to-Side NPCs ---
        for (int i = 0; i < numNPCs; i++)
        {
            // Random X position within lane width
            float xPos = Random.Range(-laneWidth, laneWidth);
            Vector3 pos = new Vector3(xPos, 0, startZ + i * zSpacing + zSpacing / 2f); // staggered

            GameObject prefab = NPCSide[i % NPCSide.Count];

            // Instantiate with prefab's original rotation and scale
            GameObject npc = Instantiate(prefab, pos, prefab.transform.rotation);
            npc.transform.localScale = prefab.transform.localScale;

            npc.tag = "NPC";
            AddNPCComponent(npc, NPC.NPCMovementType.SideToSide);
        }
    }


    private void AddNPCComponent(GameObject npc, NPC.NPCMovementType type)
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