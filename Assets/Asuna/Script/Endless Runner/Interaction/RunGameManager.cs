using System;
using System.Collections;
using UnityEngine;

public class RunGameManager : MonoBehaviour
{
    public static RunGameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }

    public event Action<GameObject> OnGameOver;

    [Header("Runner")]
    [SerializeField] private MonoBehaviour runnerMovement;

    [Header("Death Animation")]
    [SerializeField, Min(0f)]
    private float gameOverPanelDelay = 1.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void EndRun(GameObject obstacle)
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        RunnerMovement movement = runnerMovement as RunnerMovement;

        if (movement != null)
        {
            movement.PlayDeathAnimation();
        }
        else if (runnerMovement != null)
        {
            runnerMovement.enabled = false;
        }

        //Debug.Log($"Game Over! Hit obstacle: {obstacle.name}", this);

        StartCoroutine(ShowGameOverAfterDelay(obstacle));
    }

    private IEnumerator ShowGameOverAfterDelay(GameObject obstacle)
    {
        yield return new WaitForSeconds(gameOverPanelDelay);

        OnGameOver?.Invoke(obstacle);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}