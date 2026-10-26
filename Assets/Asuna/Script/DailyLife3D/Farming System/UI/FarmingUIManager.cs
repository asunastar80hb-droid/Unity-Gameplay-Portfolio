using System.Collections.Generic;
using UnityEngine;

public class FarmingUIManager : MonoBehaviour
{
    public static FarmingUIManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject farmingPanel;
    [SerializeField] private CropCardUI cropCardPrefab;
    [SerializeField] private Transform cropCardParent;

    [Header("Crops")]
    [SerializeField] private CropData[] cropData;

    private readonly Dictionary<ItemTypes, CropCardUI> _cards = new();

    private FarmPlot _currentFarmPlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void OnEnable()
    {
        if (Inventory.Instance == null)
            return;

        Inventory.Instance.OnItemChanged += HandleItemChanged;
    }

    private void OnDisable()
    {
        if (Inventory.Instance == null)
            return;

        Inventory.Instance.OnItemChanged -= HandleItemChanged;
    }

    public void Open(FarmPlot farmPlot)
    {
        if (farmPlot == null)
            return;

        _currentFarmPlot = farmPlot;

        RefreshCards();
        
        if (farmingPanel != null)
            farmingPanel.SetActive(true);
    }
    public void Close()
    {
        _currentFarmPlot = null;

        if (farmingPanel != null)
            farmingPanel.SetActive(false);
    }
    private void HandleItemChanged(ItemTypes itemType)
    {
        if (_currentFarmPlot == null)
            return;

        RefreshCards();
    }

    private void RefreshCards()
    {
        ClearCards();

        if (Inventory.Instance == null)
            return;

        foreach (CropData data in cropData)
        {
            if (data == null)
                continue;

            int seedAmount =
                Inventory.Instance.GetItemCount(
                    data.SeedItem
                );

            if (seedAmount <= 0)
                continue;

            CreateCard(data, seedAmount);
        }
    }
    private void CreateCard(
        CropData data,
        int seedAmount)
    {
        if (_cards.ContainsKey(data.SeedItem))
            return;

        if (cropCardPrefab == null)
        {
            Debug.LogError(
                "FarmingUIManager: Crop Card Prefab is missing."
            );

            return;
        }

        if (cropCardParent == null)
        {
            Debug.LogError(
                "FarmingUIManager: Crop Card Parent is missing."
            );

            return;
        }

        CropCardUI card =
            Instantiate(
                cropCardPrefab,
                cropCardParent
            );

        card.Setup(data, seedAmount);

        _cards.Add(data.SeedItem, card);
    }

    private void ClearCards()
    {
        foreach (CropCardUI card in _cards.Values)
        {
            if (card != null)
                Destroy(card.gameObject);
        }

        _cards.Clear();
    }

    public void SelectCrop(CropData cropData)
    {
        if (_currentFarmPlot == null)
            return;

        if (cropData == null)
            return;

        if (Inventory.Instance == null)
            return;

        if (!_currentFarmPlot.CanPlant(cropData))
            return;

        if (!Inventory.Instance.HasItem(
                cropData.SeedItem,
                1))
        {
            return;
        }

        bool removed =
            Inventory.Instance.RemoveItem(
                cropData.SeedItem,
                1
            );

        if (!removed)
            return;

        if (!_currentFarmPlot.TryPlant(cropData))
        {
            Inventory.Instance.AddItem(
                cropData.SeedItem,
                1
            );

            return;
        }

        Close();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}