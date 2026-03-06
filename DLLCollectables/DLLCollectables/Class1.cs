//Handles Collectable items in the game,
//including coins, speed boosts, and shields.
//Also handles scoring and awards points for collecting coins
//and handles score display through the ScoreSystem class.
//The CollectablesSpawner class is responsible for spawning collectable
//items at random intervals and positions ahead of the player, as
//well as cleaning up any collectables that fall behind the player.
//The IPlayerPowerUpTarget interface defines methods that the
//player character can implement to receive power-ups from collectables.
using System;
using UnityEngine;
using System.Collections;
namespace DLLCollectables
{
    public interface IPlayerPowerUpTarget
    {
        bool AddCoin(int amount);
        bool ActivateSpeedBoost(float multiplier, float durationSeconds);
        bool ActivateShield(float durationSeconds);
    }

    public class Collectables : MonoBehaviour
    {
        public static Collectables Instance { get; private set; } = null;
        public ScoreSystem ScoreSystem { get; private set; } = new ScoreSystem();
        
        protected bool AwakeCollectables()
        {
            if (Instance != null && Instance != this) return false;
            Instance = this;
            if (ScoreSystem == null)
            {
                ScoreSystem = new ScoreSystem();
            }
            return true;
        }

        private void Awake()
        {
            bool ok = AwakeCollectables();
            if (!ok) Destroy(gameObject);
        }
    }

    public sealed class ScoreSystem
    {
        public int Score { get; private set; }
        public event Action<int> ScoreChanged;
        public bool Reset()
        {
            Score = 0;
            ScoreChanged?.Invoke(Score);
            return true;
        }

        public bool AddPoints(int points)
        {
            if (points <= 0) return false;
            Score += points;
            ScoreChanged?.Invoke(Score);
            return true;
        }
    }
    public abstract class DLLCollectibleBase : MonoBehaviour
    {
        [Header("Scoring")]
        //coin collectable can award points when collected.
        [SerializeField] private int pointsOnCollect = 0;

        protected bool EnsureTrigger()
        {
            Collider c = GetComponent<Collider>();
            if (c == null) return false;

            c.isTrigger = true;
            return true;
        }

        protected virtual void Reset()
        {
            EnsureTrigger();
        }

        private void Awake()
        {
            EnsureTrigger();
        }

        private void OnTriggerEnter(Collider other)
        {
            IPlayerPowerUpTarget target =
                other.GetComponentInParent<IPlayerPowerUpTarget>();

            if (target == null) return;

            bool collected = OnCollected(target);
            if (!collected) return;

            if (pointsOnCollect > 0 && Collectables.Instance != null)
            {
                Collectables.Instance.ScoreSystem.AddPoints(pointsOnCollect);
            }

            Destroy(gameObject);
        }

        protected abstract bool OnCollected(
            IPlayerPowerUpTarget target
        );
    }
    public sealed class CoinCollectible : DLLCollectibleBase
    {
        [SerializeField] private int amount = 1;

        protected override bool OnCollected(IPlayerPowerUpTarget target)
        {
            if (amount <= 0) return false;
            return target.AddCoin(amount);
        }
    }

    public sealed class SpeedBoostCollectible : DLLCollectibleBase
    {
        [SerializeField] private float multiplier = 1.5f;
        [SerializeField] private float durationSeconds = 3.0f;

        protected override bool OnCollected(IPlayerPowerUpTarget target)
        {
            if (multiplier <= 1f) return false;
            if (durationSeconds <= 0f) return false;
            return target.ActivateSpeedBoost(multiplier, durationSeconds);
        }
    }

    public sealed class ShieldCollectible : DLLCollectibleBase
    {
         [SerializeField] private float durationSeconds = 5.0f;
    
         protected override bool OnCollected(IPlayerPowerUpTarget target)
         {
                if (durationSeconds <= 0f) return false;
                return target.ActivateShield(durationSeconds);
         }
    }

    public sealed class CollectablesSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;

        [Header("Prefabs (put Coin/Speed/Shield prefabs here)")]
        [SerializeField] private GameObject[] collectiblePrefabs = new GameObject[0];

        [Header("Timing")]
        [SerializeField] private float spawnIntervalSeconds = 2.0f;

        [Header("Spawn Area")]
        [SerializeField] private float spawnRangeX = 10f;
        [SerializeField] private float spawnY = 3f;

        [Tooltip("How far ahead of the player to spawn (random between min/max).")]
        [SerializeField] private float spawnAheadMinZ = 30f;
        [SerializeField] private float spawnAheadMaxZ = 80f;

        [Header("Cleanup")]
        [Tooltip("Destroy spawned collectables once they are this far behind the player.")]
        [SerializeField] private float despawnBehindDistance = 40f;

        private Coroutine spawnRoutine;

        private bool CanSpawn()
        {
            if (player == null) return false;
            if (collectiblePrefabs == null || collectiblePrefabs.Length == 0) return false;
            if (spawnIntervalSeconds <= 0f) return false;
            return true;
        }

        private bool StartSpawning()
        {
            if (!CanSpawn()) return false;

            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }

            spawnRoutine = StartCoroutine(SpawnLoop());
            return true;
        }

        private void Start()
        {
            StartSpawning();
        }

        private IEnumerator SpawnLoop()
        {
            WaitForSeconds wait = new WaitForSeconds(spawnIntervalSeconds);

            while (true)
            {
                SpawnOne();
                CleanupBehindPlayer();
                yield return wait;
            }
        }

        private bool SpawnOne()
        {
            if (!CanSpawn()) return false;

            int idx = UnityEngine.Random.Range(0, collectiblePrefabs.Length);
            GameObject prefab = collectiblePrefabs[idx];
            if (prefab == null) return false;

            float x = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX);
            float ahead = UnityEngine.Random.Range(spawnAheadMinZ, spawnAheadMaxZ);
            float z = player.position.z + ahead;

            Vector3 pos = new Vector3(x, spawnY, z);
            Instantiate(prefab, pos, Quaternion.identity);
            return true;
        }

        private bool CleanupBehindPlayer()
        {
            if (player == null) return false;

            float cutoffZ = player.position.z - despawnBehindDistance;

            
            DLLCollectibleBase[] all = FindObjectsOfType<DLLCollectibleBase>();
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] != null && all[i].transform.position.z < cutoffZ)
                {
                    Destroy(all[i].gameObject);
                }
            }

            return true;
        }
    }
}