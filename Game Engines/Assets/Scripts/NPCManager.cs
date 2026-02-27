using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    
    [Header("Forward-Backward NPCs")]
    public List<GameObject> NPCForward;

    [Header("Side-to-Side NPCs")]
    public List<GameObject> NPCSide;

    [Header("Floor Detection")]
    public GameObject floorObject;

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
    public float npcYOffset = 0f;

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

    [Header("Floor Detection")]
// This creates a list in the Inspector where you can set the size to 2 
// and drag both ground objects in.
    public GameObject[] floorObjects; 

    private void SpawnNPCs()
    {
        float spawnDistanceAhead = 50f;
        float startZ = playerTransform.position.z + spawnDistanceAhead;

        if (floorObjects == null || floorObjects.Length == 0)
        {
            Debug.LogError("No floors assigned to NPCManager!");
            return;
        }

        // --- Forward-Backward NPCs ---
        for (int i = 0; i < numNPCs; i++)
        {
            // 1. Pick a random floor from your list (Floor 1 or Floor 2)
            GameObject selectedFloor = floorObjects[Random.Range(0, floorObjects.Length)];
            
            float xPos = selectedFloor.transform.position.x;
            float surfaceY = 0f;

            // 2. Calculate the bounds of that specific floor
            Renderer floorRenderer = selectedFloor.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
                float halfWidth = (floorRenderer.bounds.size.x / 2) - 1.0f;
                
                
                // Randomize X relative to the center of the chosen floor
                xPos = Random.Range(floorRenderer.bounds.center.x - halfWidth, floorRenderer.bounds.center.x + halfWidth);
            surfaceY = floorRenderer.bounds.max.y;
            }
            float finalZ = startZ + (i * zSpacing);
            Vector3 pos = new Vector3(xPos, surfaceY + npcYOffset, startZ + i * zSpacing);

            if (i % 2 == 0) // Even numbers spawn Forward-Backward
           {
            if (NPCForward.Count > 0)
            {
                GameObject prefab = NPCForward[i % NPCForward.Count];
                GameObject npc = Instantiate(prefab, pos, prefab.transform.rotation);
                npc.transform.localScale = prefab.transform.localScale;
                npc.tag = "NPC";
                AddNPCComponent(npc, NPC.NPCMovementType.ForwardBackward);
                Debug.Log($"Spawned Forward NPC {i} at Z: {pos.z}");
            }
           }
           else // Odd numbers spawn Side-to-Side
            {
                if (NPCSide.Count > 0)
                {
                    GameObject prefab = NPCSide[i % NPCSide.Count];
                    GameObject npc = Instantiate(prefab, pos, prefab.transform.rotation);
                    npc.transform.localScale = prefab.transform.localScale;
                    npc.tag = "NPC";
                    AddNPCComponent(npc, NPC.NPCMovementType.SideToSide);
                    Debug.Log($"Spawned Side NPC {i} at Z: {pos.z}");
                }
            }
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

    private void OnDrawGizmosSelected()
   {
    // This draws a red line in your Scene view showing where the "Spawn Line" is
    Gizmos.color = Color.red;
    float rangeX = 100f; 
    float floorY = 3f;
    Vector3 start = new Vector3(-rangeX, floorY, transform.position.z);
    Vector3 end = new Vector3(rangeX, floorY, transform.position.z);
    Gizmos.DrawLine(start, end);
  }
}