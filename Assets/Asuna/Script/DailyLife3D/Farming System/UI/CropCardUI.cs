using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CropCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image icon;

    private CropData _cropData;

    public CropData CropData => _cropData;

    public void Setup(CropData cropData, int seedAmount)
    {
        if (cropData == null)
            return;

        _cropData = cropData;

        ItemData seedData = Inventory.Instance.GetItemData(cropData.SeedItem);

        if (seedData != null &&
            icon != null)
        {
            icon.sprite = seedData.Icon;
        }

        Refresh(seedAmount);
    }

    public void Refresh(int seedAmount)
    {
        if (amountText != null)
            amountText.text = seedAmount.ToString();
    }

    public void Plant()
    {
        if (_cropData == null)
            return;

        if (FarmingUIManager.Instance == null)
            return;

        FarmingUIManager.Instance.SelectCrop(_cropData);
    }
}