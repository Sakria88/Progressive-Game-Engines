using UnityEngine;
using TMPro;
using DLLCollectables;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private CollectiblesManager manager;
    [SerializeField] private TMP_Text scoreText;

    private ScoreSystem score;

    public void Initialise(CollectiblesManager managerRef)
    {
        if (managerRef == null || scoreText == null)
            return;

        score = managerRef.GetScoreSystem();
        if (score == null)
            return;

        score.ScoreChanged += OnScoreChanged;
        OnScoreChanged(score.Score);
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