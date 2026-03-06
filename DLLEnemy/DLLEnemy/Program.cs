//Spawn scheduling (intervals, waves, difficulty scaling)

//Spawn selection(enemy type, lane/position choice, weighted randomness)

//Constraints(min distance from player, avoid repeats, max alive cap)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DLLEnemy
{
    public enum NpcMovementType
    {
        SideToSide = 0,
        ForwardBackward = 1
    }

    public struct FloorInfo
    {
        public float CenterX;
        public float MinX;
        public float MaxX;
        public float SurfaceY; // bounds.max.y

        public FloorInfo(float centerX, float minX, float maxX, float surfaceY)
        {
            CenterX = centerX;
            MinX = minX;
            MaxX = maxX;
            SurfaceY = surfaceY;
        }
    }
    public struct SpawnVec3
    {
        public float X;
        public float Y;
        public float Z;

        public SpawnVec3(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }
    }
    public struct NpcSpawnRequest
    {
        public int PrefabIndex;   //prefab Unity should pick
        public SpawnVec3 Position;   // spawn position
        public NpcMovementType MovementType; // SideToSide / ForwardBackward

        public float MoveSpeed; // maps to npcScript.moveSpeed
        public float MoveRange;   // maps to npcScript.moveRange
        public float WaitTime;    // maps to npcScript.waitTime

        // For SideToSide clamping:
        public float ClampMinX;  // floor bounds min.x
        public float ClampMaxX;   // floor bounds max.x

        public NpcSpawnRequest(
            int prefabIndex,
            SpawnVec3 position,
            NpcMovementType movementType,
            float moveSpeed,
            float moveRange,
            float waitTime,
            float clampMinX,
            float clampMaxX)
        {
            PrefabIndex = prefabIndex;
            Position = position;
            MovementType = movementType;
            MoveSpeed = moveSpeed;
            MoveRange = moveRange;
            WaitTime = waitTime;
            ClampMinX = clampMinX;
            ClampMaxX = clampMaxX;
        }
    }
    public struct EnemySpawnRequest
    {
        public SpawnVec3 Position;

        public EnemySpawnRequest(SpawnVec3 position)
        {
            Position = position;
        }
    }

    public sealed class EnemyNpcSpawner
    {
        private readonly System.Random _rng;

        public EnemyNpcSpawner(int seed)
        {
            _rng = new System.Random(seed);
        }

        public EnemySpawnRequest CreateEnemySpawnBehindPlayer(
            float playerX,
            float playerY,
            float playerZ,
            float respawnBehindZ)
        {
            SpawnVec3 pos = new SpawnVec3(playerX, playerY, playerZ - respawnBehindZ);
            return new EnemySpawnRequest(pos);
        }

        public List<NpcSpawnRequest> CreateNpcSpawns(
            List<FloorInfo> floors,
            int count,
            int prefabCount,
            float playerZ,
            float spawnDistanceAhead,
            float zSpacing,
            float npcLaneWidth,
            float npcYOffset,
            float moveSpeed,
            float moveRange,
            float waitTime,
            bool alternateTypes)
        {
            List<NpcSpawnRequest> result = new List<NpcSpawnRequest>(Math.Max(0, count));

            if (floors == null || floors.Count == 0) return result;
            if (count <= 0) return result;
            if (prefabCount <= 0) return result;

            for (int i = 0; i < count; i++)
            {
                FloorInfo floor = floors[_rng.Next(0, floors.Count)];

                // X within center +/- npcLaneWidth (same as NPCManager)
                float x = NextRange(floor.CenterX - npcLaneWidth, floor.CenterX + npcLaneWidth);

                float y = floor.SurfaceY + npcYOffset;
                float z = playerZ + spawnDistanceAhead + (i * zSpacing);

                int prefabIndex = i % prefabCount;

                NpcMovementType type;
                if (alternateTypes)
                {
                    type = (i % 2 == 0) ? NpcMovementType.SideToSide : NpcMovementType.ForwardBackward;
                }
                else
                {
                    type = (_rng.Next(0, 2) == 0) ? NpcMovementType.SideToSide : NpcMovementType.ForwardBackward;
                }

                NpcSpawnRequest req = new NpcSpawnRequest(
                    prefabIndex,
                    new SpawnVec3(x, y, z),
                    type,
                    moveSpeed,
                    moveRange,
                    waitTime,
                    floor.MinX,
                    floor.MaxX
                );

                result.Add(req);
            }

            return result;
        }

        private float NextRange(float min, float max)
        {
            if (max < min)
            {
                float t = min;
                min = max;
                max = t;
            }

            double r = _rng.NextDouble(); // [0,1)
            return (float)(min + (max - min) * r);
        }
    }
}
