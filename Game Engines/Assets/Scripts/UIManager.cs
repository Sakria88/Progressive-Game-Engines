// This script manages the user interface (UI) elements that 
//display the player's current coin count, score, and speed. 
//It references the PlayerCharacter to get the necessary data 
//and updates the UI text elements accordingly. The UI is refreshed 
//every frame in the Update method to ensure it reflects the latest 
//game state.
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DLLCollectables;
using System.Collections;
using DLLGameManager;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private ScoreUI scoreUI;
    [SerializeField] private CollectiblesManager collectiblesManager;
    [Header("References")]
    [SerializeField] private PlayerCharacter player;

    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI speedText;
    
    [SerializeField] private GameObject pausePanel;

    [Header("Boost UI")]
    [SerializeField] private GameObject boostPanel;
    [SerializeField] private TextMeshProUGUI boostText;

    [Header("Game Over UI")]
    [SerializeField] private TextMeshProUGUI coinsCollectedText;
    [SerializeField] private TextMeshProUGUI oldScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI newScoreText;

    private int previousScore = 0;
    private int lastRunScore = 0;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (scoreUI != null && collectiblesManager != null)
        {
            scoreUI.Initialise(collectiblesManager);
        }
        
        RefreshAllUI();
        
        
    }

    private void Update()
    {
        if (player == null) return;

        RefreshAllUI();
    }

    private void RefreshAllUI()
    {
        UpdateCoinsUI();
        UpdateScoreUI();
        UpdateSpeedUI();
        
    }

    private void UpdateCoinsUI()
    {
        if (coinText == null) return;
        coinText.text = "Coins: " + CollectiblesManager.Instance.coinCount.ToString();
        Debug.Log($"[Collectible] Coin Collected | Total Coins = {player.TotalCoinsCollected}");
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null) return;

        // Get current score from the collectibles manager's score system
        var scoreSystem = collectiblesManager.GetScoreSystem();
        if (scoreSystem != null)
        {
            scoreText.text = "Score: " + scoreSystem.Score;
        }
    }
    public void RefreshSpeedUI()
    {
        UpdateSpeedUI();
    }
    private void UpdateSpeedUI()
    {
        if (speedText == null) return;
        float baseSpeed = player.NormalMoveSpeed;
        float currentSpeed = player.CurrentMoveSpeed;
        
        speedText.text = $"Speed: {currentSpeed:F1}";
        
        
    }
    
    public void UpdateUI(GameState state)
    {
        if (pausePanel != null)
            pausePanel.SetActive(state == GameState.Paused);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(state == GameState.GameOver);
        if (state == GameState.GameOver)
        {
            UpdateGameOverUI();
        }
    }
    private void UpdateGameOverUI()
    {
        if (collectiblesManager == null) return;

        var scoreSystem = collectiblesManager.GetScoreSystem();

        int coins = CollectiblesManager.Instance.coinCount;
        int currentScore = scoreSystem != null ? scoreSystem.Score : 0;
         // Save score before reset
        lastRunScore = currentScore;

        if (coinsCollectedText != null)
            coinsCollectedText.text = "Coins Collected: " + coins;

        if (oldScoreText != null)
            oldScoreText.text = "Old Score: " + previousScore;

        if (newScoreText != null)
            newScoreText.text = "New Score: " + currentScore;

        previousScore = currentScore;
    }
    
    public void ShowBoostMessage(string message, float duration)
    {
        StartCoroutine(BoostMessageRoutine(message, duration));
    }

    private IEnumerator BoostMessageRoutine(string message, float duration)
    {
        boostPanel.SetActive(true);
        boostText.text = message;

        yield return new WaitForSeconds(duration);

        boostPanel.SetActive(false);
    }

    
}