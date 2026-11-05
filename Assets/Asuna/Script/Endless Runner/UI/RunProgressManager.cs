
using UnityEngine;
using TMPro;

public class RunProgressManager : MonoBehaviour
{
    public static RunProgressManager Instance { get; private set; }

    [Header("Runner")]
    [SerializeField] private RunnerMovement runner;

    [Header("Speed")]
    [SerializeField, Min(0f)] private float speedIncreasePerSecond = 0.15f;
    [SerializeField, Min(0f)] private float maxSpeed = 18f;

    [Header("Distance UI")]
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text finalDistanceText;
    [SerializeField] private TMP_Text bestDistanceText;

    private float startZ;
    private float distance;
    private float bestDistance;
    private bool initialized;

    private const string BestDistanceKey = "Run_BestDistance";

    public float Distance => distance;
    public float BestDistance => bestDistance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        bestDistance = PlayerPrefs.GetFloat(BestDistanceKey, 0f);
    }

    private void Start()
    {
        if (runner == null)
        {
            Debug.LogError("RunProgressManager: Runner is not assigned.", this);
            enabled = false;
            return;
        }

        startZ = runner.transform.position.z;
        initialized = true;

        UpdateDistanceUI();
    }

    private void Update()
    {
        if (!initialized || RunGameManager.Instance == null)
            return;

        if (RunGameManager.Instance.IsGameOver)
            return;

        float newSpeed = Mathf.Min(
            runner.ForwardSpeed + speedIncreasePerSecond * Time.deltaTime,
            maxSpeed
        );

        runner.SetForwardSpeed(newSpeed);
        
        distance = Mathf.Max(
            distance,
            runner.transform.position.z - startZ
        );

        UpdateDistanceUI();
    }

    private void UpdateDistanceUI()
    {
        if (distanceText != null)
            distanceText.text = $"{Mathf.FloorToInt(distance)} m";

        if (bestDistanceText != null)
            bestDistanceText.text = $" {Mathf.FloorToInt(bestDistance)} m";
    }

    public void SaveRunResult()
    {
        if (distance > bestDistance)
        {
            bestDistance = distance;
            PlayerPrefs.SetFloat(BestDistanceKey, bestDistance);
            PlayerPrefs.Save();
        }

        if (finalDistanceText != null)
            finalDistanceText.text = $"{Mathf.FloorToInt(distance)} m";

        if (bestDistanceText != null)
            bestDistanceText.text = $" {Mathf.FloorToInt(bestDistance)} m";
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
