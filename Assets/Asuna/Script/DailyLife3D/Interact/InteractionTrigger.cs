using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Collider))]
public class InteractionTrigger : MonoBehaviour
{
    private Interactable _interactable;
    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        InteractionManager.Instance.Register(_interactable);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        InteractionManager.Instance.Unregister(_interactable);
    }
}