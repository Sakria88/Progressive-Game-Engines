using System;

using UnityEngine;

namespace DLLGameCollectables
{
    public interface IPlayerPowerUpTarget
    {
         bool AddCoin(int amount);
         bool ActivateSpeedBoost(float multiplier, float durationSeconds);
         bool ActivateShield(float durationSeconds);
    }

    public class Collectables : MonoBehaviour
    {
        public static Collectables? Instance { get; private set; }
        public ScoreSystem ScoreSystem { get; private set; } = new ScoreSystem();
        // Add your spawner fields here if you want this script to do spawning
        // public GameObject[] collectiblePrefabs;
        // public float spawnInterval = 2f;
        protected bool AwakeCollectables()
        {
            if (Instance != null && Instance != this) return false;
            Instance = this;
            if (ScoreSystem == null) ScoreSystem = new ScoreSystem();
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

        public event Action<int>? ScoreChanged;

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

    public abstract class DllCollectibleBase : MonoBehaviour
    {
        [Header("Scoring")]
        [SerializeField] private int pointsOnCollect = 0;

        protected virtual bool EnsureTrigger()
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
            // Player must implement the DLL interface
            IPlayerPowerUpTarget target = other.GetComponent<IPlayerPowerUpTarget>();
            if (target == null) return;

            bool collected = OnCollected(target);
            if (!collected) return;

            // Use YOUR existing singleton + ScoreSystem
            if (pointsOnCollect > 0 && Collectables.Instance != null)
            {
                Collectables.Instance.ScoreSystem.AddPoints(pointsOnCollect);
            }

            Destroy(gameObject);
        }

        protected abstract bool OnCollected(IPlayerPowerUpTarget target);
    }
}



