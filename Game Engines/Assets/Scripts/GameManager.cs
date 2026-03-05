//Game over State Screen and potentially Pause state Screen
// Also handles respawning the player after game over
//and resetting collectibles/score.
using UnityEngine;
using System.Collections;
using DLLGameManager;



public class GameManager : MonoBehaviour
{
    [SerializeField] private CollisionHandler collisionHandler;
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float gameOverDelay = 5f;
    public static GameManager Instance;

    //logic handled by the DLL
    private GameStateController _stateController;
    // Property to expose the DLL state to the rest of Unity
    public GameState CurrentState => _stateController.CurrentState;
    // public enum GameState
    // {
    //     Playing,
    //     Paused,
    //     GameOver
    // }
    // public static GameManager Instance;

    // public GameState CurrentState { get; private set; }

    private void Awake()
    {
        Instance = this;
        // Initialize DLL logic
        _stateController = new GameStateController();
    }

    private void Start()
    {
        if (collisionHandler == null)
        {
            collisionHandler = FindObjectOfType<CollisionHandler>();
        }

        SetState(GameState.Playing);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
                PauseGame();
            else if (CurrentState == GameState.Paused)
                ResumeGame();
        }
    }
    

    public void SetState(GameState newState)
    {
        // CurrentState = newState;
        _stateController.SetState(newState);
        UIManager.Instance.UpdateUI(newState);
    }

    //For Pause state if i made one.
    public void PauseGame()
    {
        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    //For Resume state if i made one.
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        Time.timeScale = 0f;

        SetState(GameState.GameOver);
        //realtime = timescale 0, so this will wait in real time, not game time.
        yield return new WaitForSecondsRealtime(gameOverDelay);
        if (collisionHandler != null) 
        {
            collisionHandler.TriggerRespawn();
        }
        // Reset score/collectibles in DLL and manager
        _stateController.ResetSession();
        Time.timeScale = 1f;

        SetState(GameState.Playing);
    }

    // private void RespawnPlayer()
    // {
    //     if (player == null || respawnPoint == null) return;

    //     player.transform.position = respawnPoint.position;
    // }
}