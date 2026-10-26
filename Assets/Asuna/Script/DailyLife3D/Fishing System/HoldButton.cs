using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    public bool isHolding { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }
}
