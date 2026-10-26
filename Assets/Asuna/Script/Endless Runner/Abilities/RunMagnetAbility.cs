using UnityEngine;

public class RunMagnetAbility : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField, Min(0.1f)] private float pullRadius = 8f;
    [SerializeField, Min(0.1f)] private float pullSpeed = 12f;

    private readonly Collider[] _results = new Collider[64];

    private float _remainingTime;

    public bool IsActive => _remainingTime > 0f;
    public float RemainingTime => _remainingTime;

    public void Activate(float duration)
    {
        if (duration <= 0f)
            return;

        _remainingTime = duration;
      //  Debug.Log($"Magnet activated for {duration} seconds.", this);
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
         //   Debug.Log("Magnet expired.", this);
            return;
        }

        AttractCoins();
    }

    private void AttractCoins()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            pullRadius,
            _results
        );

        for (int i = 0; i < count; i++)
        {
            Collider hit = _results[i];

            if (hit == null)
                continue;

            RunCollectible coin =
                hit.GetComponentInParent<RunCollectible>();

            if (coin == null)
                continue;

            coin.AttractTo(transform, pullSpeed);
        }

        System.Array.Clear(_results, 0, count);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}