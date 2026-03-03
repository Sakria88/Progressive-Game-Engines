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
    public enum EnemyKind
    {
        Enemy, // main chaser (class name in Unity is Enemy)
        NPC_SideToSide,
        NPC_ForwardBackward
    }

    public readonly struct Float3
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Float3(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }
    }

    public readonly struct SpawnRequest
    {
        public readonly EnemyKind Kind;
        public readonly Float3 Position;

        // Optional per-spawn parameters (Unity behaviour can use these)
        public readonly float Speed;
        public readonly float DelaySeconds;

        public SpawnRequest(EnemyKind kind, Float3 position, float speed, float delaySeconds)
        {
            Kind = kind;
            Position = position;
            Speed = speed;
            DelaySeconds = delaySeconds;
        }
    }
    public sealed class EnemySpawnConfig
    {
        // Floor / lane space
        public float FloorCenterX { get; set; }
        public float LaneWidth { get; set; }
        public float EdgePadding { get; set; }

        // Spawn distances
        public float SpawnZOffset { get; set; }          // NPC spawns ahead of player
        public float EnemyRespawnBehind { get; set; }    // enemy respawns behind player

        // Timing / caps
        public float NpcSpawnIntervalSeconds { get; set; }
        public int MaxNpcsAlive { get; set; }

        // Speeds
        public float EnemyChaseSpeed { get; set; }
        public float NpcSideToSideSpeed { get; set; }
        public float NpcForwardBackwardSpeed { get; set; }

        // Enemy delay requirement
        public float EnemyWaitSeconds { get; set; }

        public EnemySpawnConfig()
        {
            // sensible defaults
            FloorCenterX = 0f;
            LaneWidth = 11f;
            EdgePadding = 0.75f;

            SpawnZOffset = 60f;
            EnemyRespawnBehind = 25f;

            NpcSpawnIntervalSeconds = 2.0f;
            MaxNpcsAlive = 8;

            EnemyChaseSpeed = 18f;
            NpcSideToSideSpeed = 4f;
            NpcForwardBackwardSpeed = 3.5f;

            EnemyWaitSeconds = 5.0f;
        }
    }
}
