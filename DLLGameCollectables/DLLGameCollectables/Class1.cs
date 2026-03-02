using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DLLGameCollectables
{
    public class Collectables : MonoBehaviour
    {
        public interface IPlayerPowerUpTarget
        {
            bool AddCoin(int amount);
            bool ActivateSpeedBoost(float multiplier, float durationSeconds);
            bool ActivateShield(float durationSeconds);
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
}
