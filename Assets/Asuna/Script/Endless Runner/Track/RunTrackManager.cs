
using System.Collections.Generic;
using UnityEngine;

public class RunTrackManager : MonoBehaviour
{
    public static RunTrackManager Instance { get; private set; }

    [Header("Initial Tile")]
    [SerializeField] private RunTrackSegment currentTile;

    [Header("Tile Prefabs")]
    [SerializeField] private List<RunTrackSegment> tilePrefabs = new();

    private RunTrackSegment previousTile;
    private bool isReplacingTile;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (currentTile == null)
        {
            Debug.LogError("RunTrackManager: Initial tile is missing.", this);
            return;
        }

        SpawnNextTile();
    }

    public void OnPlayerEnteredTile(RunTrackSegment enteredTile)
    {
        if (enteredTile == null || enteredTile == currentTile)
            return;

        if (isReplacingTile)
            return;

        isReplacingTile = true;

        previousTile = currentTile;
        currentTile = enteredTile;

        if (previousTile != null)
            Destroy(previousTile.gameObject , 3);

        SpawnNextTile();

        isReplacingTile = false;
    }

    private void SpawnNextTile()
    {
        if (tilePrefabs.Count == 0)
        {
            Debug.LogError("RunTrackManager: Tile Prefab list is empty.", this);
            return;
        }

        if (currentTile == null || currentTile.EndPoint == null)
        {
            Debug.LogError("RunTrackManager: Current tile or EndPoint is missing.", this);
            return;
        }

        RunTrackSegment prefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];

        if (prefab == null || prefab.StartPoint == null)
        {
            Debug.LogError("RunTrackManager: Invalid tile Prefab or StartPoint.", this);
            return;
        }

        RunTrackSegment newTile = Instantiate(prefab);

        Vector3 offset = currentTile.EndPoint.position - newTile.StartPoint.position;

        newTile.transform.position += offset;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
