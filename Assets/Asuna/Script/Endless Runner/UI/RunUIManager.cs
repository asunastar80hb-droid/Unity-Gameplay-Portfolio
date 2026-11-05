using TMPro;
using UnityEngine;

public class RunUIManager : MonoBehaviour
{
    [Header("Score UI")]
    [SerializeField] private TMP_Text coinCountText;
    [SerializeField] private TMP_Text finalCoinCountText;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateCoinCount(0);

        if (RunScoreManager.Instance != null)
            RunScoreManager.Instance.OnScoreChanged += UpdateCoinCount;

        if (RunGameManager.Instance != null)
            RunGameManager.Instance.OnGameOver += ShowGameOver;
    }

    private void UpdateCoinCount(int score)
    {
        if (coinCountText != null)
            coinCountText.text = $"{score}";

        if (finalCoinCountText != null)
            finalCoinCountText.text = $"{score}";
    }

    private void ShowGameOver(GameObject obstacle)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (RunScoreManager.Instance != null)
            UpdateCoinCount(RunScoreManager.Instance.Score);
    }

    private void OnDestroy()
    {
        if (RunScoreManager.Instance != null)
            RunScoreManager.Instance.OnScoreChanged -= UpdateCoinCount;

        if (RunGameManager.Instance != null)
            RunGameManager.Instance.OnGameOver -= ShowGameOver;
    }
}