using UnityEngine;

public class RunObstacle : RunInteractable
{
    private bool _hasActivated;

    public override void Interact(GameObject runner)
    {
        if (_hasActivated)
            return;

        if (RunGameManager.Instance == null)
        {
            Debug.LogError("RunObstacle: RunGameManager not found.", this);
            return;
        }

        if (RunGameManager.Instance.IsGameOver)
            return;

        RunShieldAbility shield = runner.GetComponentInParent<RunShieldAbility>();

        if (shield != null && shield.TryAbsorbHit())
        {
            _hasActivated = true;
           // Debug.Log("Obstacle hit absorbed by shield.", this);
            return;
        }

        _hasActivated = true;
        RunGameManager.Instance.EndRun(gameObject);
    }
}