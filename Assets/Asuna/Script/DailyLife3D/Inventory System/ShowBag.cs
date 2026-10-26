using UnityEngine;

public class ShowBag : Interactable
{
    [SerializeField] private GameObject bagCanvas;

    public override void Interact()
    {
        OpenBag();
    }

    public void OpenBag()
    {
        if (bagCanvas == null)
            return;

        bagCanvas.SetActive(true);
    }

    public void CloseBag()
    {
        if (bagCanvas == null)
            return;

        bagCanvas.SetActive(false);
    }
}