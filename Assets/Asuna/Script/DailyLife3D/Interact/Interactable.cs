using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected string buttonText = "Interact";
    public string ButtonText => buttonText;

    public abstract void Interact();
}
