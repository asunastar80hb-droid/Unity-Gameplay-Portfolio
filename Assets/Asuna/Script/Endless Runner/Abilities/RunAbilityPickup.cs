using UnityEngine;

public class RunAbilityPickup : RunInteractable
{
    public enum AbilityType
    {
        Magnet,
        Shield
    }

    [Header("Ability Settings")]
    [SerializeField] private AbilityType abilityType;
    [SerializeField, Min(0.1f)] private float duration = 5f;

    private bool _isCollected;

    public override void Interact(GameObject runner)
    {
        if (_isCollected || runner == null)
            return;

        if (RunGameManager.Instance != null &&
            RunGameManager.Instance.IsGameOver)
            return;

        bool activated = false;

        switch (abilityType)
        {
            case AbilityType.Magnet:
            {
                RunMagnetAbility magnet =
                    runner.GetComponentInParent<RunMagnetAbility>();

                if (magnet == null)
                    break;

                magnet.Activate(duration);
                activated = true;
                break;
            }

            case AbilityType.Shield:
            {
                RunShieldAbility shield =
                    runner.GetComponentInParent<RunShieldAbility>();

                if (shield == null)
                    break;

                shield.Activate(duration);
                activated = true;
                break;
            }
        }

        if (!activated)
        {
            Debug.LogWarning(
                $"Could not activate {abilityType}. " +
                "Check the ability component on the player.",
                this
            );
            return;
        }

        _isCollected = true;
        Destroy(gameObject);
    }
}