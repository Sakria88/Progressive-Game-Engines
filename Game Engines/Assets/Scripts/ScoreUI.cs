using UnityEngine;
using TMPro;
using DLLGameCollectables;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private CollectiblesManager manager;
    [SerializeField] private TMP_Text scoreText;

    private ScoreSystem score;

    private bool StartInit()
    {
        if (manager == null) manager = FindFirstObjectByType<CollectiblesManager>();
        if (manager == null || scoreText == null) return false;

        score = manager.GetScoreSystem();
        if (score == null) return false;

        score.ScoreChanged += OnScoreChanged;
        OnScoreChanged(score.Score);
        return true;
    }

    private bool Start()
    {
        return StartInit();
    }

    private void OnDestroy()
    {
        if (score != null) score.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}