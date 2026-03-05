//(Game over State Screen , Pause state Screen)
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour
{
    [SerializeField] private CollisionHandler collisionHandler;
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float gameOverDelay = 5f;
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        collisionHandler = FindObjectOfType<CollisionHandler>();
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
        CurrentState = newState;
        UIManager.Instance.UpdateUI(newState);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

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

        yield return new WaitForSecondsRealtime(gameOverDelay);
        if (collisionHandler != null) 
        {
            collisionHandler.TriggerRespawn();
        }

        Time.timeScale = 1f;

        SetState(GameState.Playing);
    }

    // private void RespawnPlayer()
    // {
    //     if (player == null || respawnPoint == null) return;

    //     player.transform.position = respawnPoint.position;
    // }
}