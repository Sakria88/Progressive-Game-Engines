using System.Collections.Generic;
using UnityEngine;
using DLLEnemy;

public class DLLEnemyBridge : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Enemy")]
    [SerializeField] private EnemyCharacter enemy; // your existing enemy in-scene
    [SerializeField] private float respawnBehindZ = 25f;

    [Header("NPC Spawning")]
    [SerializeField] private List<GameObject> npcPrefabs = new List<GameObject>();
    [SerializeField] private Transform npcContainer;

    [Header("Floors")]
    [SerializeField] private GameObject[] floorObjects;

    [Header("Spawn Settings")]
    [SerializeField] private int countPerType = 6;
    [SerializeField] private float spawnDistanceAhead = 50f;
    [SerializeField] private float zSpacing = 12f;
    [SerializeField] private float npcLaneWidth = 3f;
    [SerializeField] private float npcYOffset = 0.05f;

    [Header("NPC Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float moveRange = 2f;
    [SerializeField] private float waitTime = 0.5f;

    private EnemyNpcSpawner dllSpawner;

    private void Awake()
    {
        dllSpawner = new EnemyNpcSpawner(System.Environment.TickCount);

        if (npcContainer == null)
        {
            GameObject go = new GameObject("NPC_Container");
            npcContainer = go.transform;
        }
    }

    private void Start()
    {
        if (player == null) return;

        // ===== 1) Spawn/position enemy behind player once =====
        if (enemy != null)
        {
            EnemySpawnRequest req = dllSpawner.CreateEnemySpawnBehindPlayer(
                player.position.x,
                player.position.y,
                player.position.z,
                respawnBehindZ
            );

            enemy.transform.position = new Vector3(req.Position.X, req.Position.Y, req.Position.Z);

            // EnemyCharacter already waits chaseDelaySeconds in Awake()
            // and ResetEnemy() waits again, so we don't need DLL logic for that.
        }

        // ===== 2) Spawn NPCs =====
        SpawnNpcsFromDll();
    }

    private void SpawnNpcsFromDll()
    {
        if (player == null) return;
        if (floorObjects == null || floorObjects.Length == 0) return;
        if (npcPrefabs == null || npcPrefabs.Count == 0) return;

        List<FloorInfo> floors = BuildFloorInfo();
        if (floors.Count == 0) return;

        List<NpcSpawnRequest> spawns = dllSpawner.CreateNpcSpawns(
            floors,
            countPerType,
            npcPrefabs.Count,
            player.position.z,
            spawnDistanceAhead,
            zSpacing,
            npcLaneWidth,
            npcYOffset,
            moveSpeed,
            moveRange,
            waitTime,
            alternateTypes: false
        );

        for (int i = 0; i < spawns.Count; i++)
        {
            NpcSpawnRequest s = spawns[i];

            GameObject prefab = npcPrefabs[s.PrefabIndex];
            if (prefab == null) continue;

            Vector3 pos = new Vector3(s.Position.X, s.Position.Y, s.Position.Z);
            GameObject npc = Instantiate(prefab, pos, prefab.transform.rotation, npcContainer);

            NPCCharacter npcChar = npc.GetComponent<NPCCharacter>();
            if (npcChar == null) continue;

            npcChar.moveSpeed = s.MoveSpeed;
            npcChar.moveRange = s.MoveRange;
            npcChar.waitTime = s.WaitTime;

            // Convert DLL enum to your NPC enum
            npcChar.movementType =
                (s.MovementType == NpcMovementType.SideToSide)
                    ? NPCCharacter.NPCMovementType.SideToSide
                    : NPCCharacter.NPCMovementType.ForwardBackward;

            if (npcChar.movementType == NPCCharacter.NPCMovementType.SideToSide)
            {
                npcChar.SetSideToSideLimits(s.ClampMinX, s.ClampMaxX);
            }
        }
    }

    private List<FloorInfo> BuildFloorInfo()
    {
        List<FloorInfo> floors = new List<FloorInfo>();

        for (int i = 0; i < floorObjects.Length; i++)
        {
            GameObject f = floorObjects[i];
            if (f == null) continue;

            Renderer r = f.GetComponentInChildren<Renderer>();
            if (r == null) continue;

            floors.Add(new FloorInfo(
                r.bounds.center.x,
                r.bounds.min.x,
                r.bounds.max.x,
                r.bounds.max.y
            ));
        }

        return floors;
    }
}