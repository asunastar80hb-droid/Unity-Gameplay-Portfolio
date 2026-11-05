using System;
using UnityEngine;

public class RunScoreManager : MonoBehaviour
{
    public static RunScoreManager Instance { get; private set; }

    public int Score { get; private set; }

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddScore(int amount)
    {
        if (amount <= 0)
            return;

        if (RunGameManager.Instance != null &&
            RunGameManager.Instance.IsGameOver)
            return;

        Score += amount;

        OnScoreChanged?.Invoke(Score);
        // Debug.Log($"Run Score: {Score}", this);
    }

    public void ResetScore()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}