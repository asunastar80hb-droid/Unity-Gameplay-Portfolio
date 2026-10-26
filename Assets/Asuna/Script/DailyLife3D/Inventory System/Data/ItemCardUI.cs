using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Image icon;

    private ItemTypes _itemType;

    public ItemTypes ItemType => _itemType;

    public void Setup(InventoryItem item)
    {
        if (item == null)
            return;

        _itemType = item.ItemType;

        if (icon != null)
            icon.sprite = item.Data.Icon;

        Refresh(item.Amount);
    }

    public void Refresh(int amount)
    {
        if (valueText != null)
            valueText.text = amount.ToString();
    }
}