using UnityEngine;

public class RunShieldAbility : MonoBehaviour
{
    private float _remainingTime;

    public bool IsActive => _remainingTime > 0f;
    public float RemainingTime => _remainingTime;

    public void Activate(float duration)
    {
        if (duration <= 0f)
            return;

        _remainingTime = duration;

        //Debug.Log($"Shield activated for {duration} seconds.", this);
    }

    public bool TryAbsorbHit()
    {
        if (!IsActive)
            return false;

        _remainingTime = 0f;

       // Debug.Log("Shield absorbed one hit.", this);
        return true;
    }

    private void Update()
    {
        if (!IsActive)
            return;

        if (RunGameManager.Instance != null &&
            RunGameManager.Instance.IsGameOver)
            return;

        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0f)
        {
            _remainingTime = 0f;
          //  Debug.Log("Shield expired.", this);
        }
    }
}