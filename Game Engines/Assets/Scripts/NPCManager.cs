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

    private void Start()
    {
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