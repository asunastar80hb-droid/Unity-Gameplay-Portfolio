using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RunInteractionTrigger : MonoBehaviour
{
    [SerializeField] private RunInteractable interactable;
    [SerializeField] private string runnerTag = "Player";

    private bool _hasTriggered;

    private void Awake()
    {
        Collider triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;

        if (interactable == null)
            interactable = GetComponent<RunInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered)
            return;

        if (!other.CompareTag(runnerTag))
            return;

        if (interactable == null)
        {
            Debug.LogWarning("RunInteractionTrigger: No RunInteractable found.", this);
            return;
        }

        _hasTriggered = true;
        interactable.Interact(other.gameObject);
    }
}