using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private InteractionButton buttonPrefab;
    [SerializeField] private Transform buttonParent;

    private readonly Dictionary<Interactable, InteractionButton> _activeInteractions = new Dictionary<Interactable, InteractionButton>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(Interactable interactable)
    {
        if (interactable == null)
            return;

        if (_activeInteractions.ContainsKey(interactable))
            return;

        CreateButton(interactable);
    }

    public void Unregister(Interactable interactable)
    {
        if (interactable == null)
            return;

        if (!_activeInteractions.TryGetValue(interactable, out InteractionButton button))
            return;

        if (button != null)
            Destroy(button.gameObject);

        _activeInteractions.Remove(interactable);
    }

    private void CreateButton(Interactable interactable)
    {
        if (buttonPrefab == null)
        {
            Debug.LogError("InteractionManager: Button Prefab is not assigned.");
            return;
        }

        if (buttonParent == null)
        {
            Debug.LogError("InteractionManager: Button Parent is not assigned.");
            return;
        }

        InteractionButton button = Instantiate(buttonPrefab, buttonParent);

        button.Setup(interactable, interactable.ButtonText);

        _activeInteractions.Add(interactable, button);
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}