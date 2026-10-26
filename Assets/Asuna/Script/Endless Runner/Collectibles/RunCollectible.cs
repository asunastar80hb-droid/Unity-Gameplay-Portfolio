using UnityEngine;

public class RunCollectible : RunInteractable
{
    [Header("Collectible Settings")]
    [SerializeField, Min(1)] private int scoreValue = 1;
    
    
    private Transform _magnetTarget;
    private float _magnetSpeed;
    private bool _isCollected;

    public override void Interact(GameObject runner)
    {
        if (_isCollected)
            return;

        if (RunGameManager.Instance != null &&
            RunGameManager.Instance.IsGameOver)
            return;

        if (RunScoreManager.Instance == null)
        {
            Debug.LogError("RunCollectible: RunScoreManager not found.", this);
            return;
        }

        _isCollected = true;

        RunScoreManager.Instance.AddScore(scoreValue);

      //  Debug.Log($"Collected {gameObject.name}: +{scoreValue}", this);

        Destroy(gameObject);
    }
    
    public void AttractTo(Transform target, float speed)
    {
        if (_isCollected || target == null)
            return;

        _magnetTarget = target;
        _magnetSpeed = speed;
    }
    
    private void Update()
    {
        if (_isCollected || _magnetTarget == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            _magnetTarget.position,
            _magnetSpeed * Time.deltaTime
        );
    }


}