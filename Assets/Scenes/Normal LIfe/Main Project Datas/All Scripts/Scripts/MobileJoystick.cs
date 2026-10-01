using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick  : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform handle;
    public RectTransform background;
    public float handleRange = 50f;

    Vector2 input;
    RectTransform rect;

    void Awake()
    {
        rect = background != null ? background : (RectTransform)transform;
    }

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos = Vector2.ClampMagnitude(pos, handleRange);
            handle.anchoredPosition = pos;
            input = pos / handleRange;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public Vector2 GetInput() => input;
}