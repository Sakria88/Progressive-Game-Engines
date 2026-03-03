// This script manages the user interface (UI) elements that 
//display the player's current coin count, score, and speed. 
//It references the PlayerCharacter to get the necessary data 
//and updates the UI text elements accordingly. The UI is refreshed 
//every frame in the Update method to ensure it reflects the latest 
//game state.
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScoreUI scoreUI;
    [SerializeField] private CollectiblesManager collectiblesManager;
    [Header("References")]
    [SerializeField] private PlayerCharacter player;

    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI speedText;
    

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
        // coinText.text = "Coins: " + player.TotalCoinsCollected.ToString();
        Debug.Log($"[Collectible] Coin Collected | Total Coins = {player.TotalCoinsCollected}");
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null) return;

        // Replace this once you add a Score variable to PlayerCharacter.
        // scoreText.text = "Score: " + player.TotalScore.ToString();
        // scoreText.text = "Score: 0";
        var scoreSystem = collectiblesManager.GetScoreSystem();
        if (scoreSystem != null)
        {
            scoreText.text = "Score: " + scoreSystem.Score;
        }
    }

    private void UpdateSpeedUI()
    {
        if (speedText == null) return;

        // Replace this once you have speed info exposed (e.g., current forward speed).
        speedText.text = "Speed: " + player.CurrentSpeed.ToString("0");
        
    }

    
}