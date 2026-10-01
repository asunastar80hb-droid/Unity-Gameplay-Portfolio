using UnityEngine;

public class BagCount : MonoBehaviour
{
    [SerializeField] private int itemsPerRow = 7;
    [SerializeField] private int minimumRows = 3;
    [SerializeField] private float rowHeight = 334f;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Refresh()
    {
        if (_rectTransform == null)
            return;

        int itemCount = _rectTransform.childCount;

        int rowCount = Mathf.CeilToInt((float)itemCount / itemsPerRow);
        rowCount = Mathf.Max(rowCount, minimumRows);

        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, rowCount * rowHeight);
    }
}