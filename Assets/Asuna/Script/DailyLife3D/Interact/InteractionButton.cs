using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    private Interactable _interactable;

    public void Setup(Interactable interactable, string buttonText)
    {
        _interactable = interactable;
        text.text = buttonText;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        if (_interactable == null)
            return;

        _interactable.Interact();
    }
}