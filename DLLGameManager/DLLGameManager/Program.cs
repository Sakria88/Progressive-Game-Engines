using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Handles the the data and state transitions
namespace DLLGameManager
{
    public enum GameState { Playing, Paused, GameOver }

    public class GameStateController
    {
        public GameState CurrentState { get; private set; }
        public int Score { get; private set; }
        public int Collectibles { get; private set; }

        public void SetState(GameState newState)
        {
            CurrentState = newState;
        }

        public void AddScore(int amount)
        {
            Score += amount;
        }

        public void ResetSession()
        {
            Score = 0;
            Collectibles = 0;
            CurrentState = GameState.Playing;
        }
    }
}
