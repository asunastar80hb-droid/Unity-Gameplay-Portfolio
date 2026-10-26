using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUpgradeUI : MonoBehaviour
{
    public static FurnitureUpgradeUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panel;

    [SerializeField] private TMP_Text furnitureNameText;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;

    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text currentCoinText;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;

    private FurnitureInstance _currentFurniture;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (panel != null)
            panel.SetActive(false);

        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradeClicked);

        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    private void OnEnable()
    {
        if (Inventory.Instance != null)
            Inventory.Instance.OnItemChanged += OnInventoryChanged;
    }

    private void OnDisable()
    {
        if (Inventory.Instance != null)
            Inventory.Instance.OnItemChanged -= OnInventoryChanged;
    }

    public void Open(FurnitureInstance furniture)
    {
        if (furniture == null)
            return;

        if (!furniture.CanUpgrade)
            return;

        _currentFurniture = furniture;

        RefreshUI();

        if (panel != null)
            panel.SetActive(true);
    }

    private void RefreshUI()
    {
        if (_currentFurniture == null)
            return;

        FurnitureData data = _currentFurniture.FurnitureData;

        FurnitureLevelData nextLevel = _currentFurniture.NextLevelData;

        if (data == null || nextLevel == null)
        {
            Close();
            return;
        }

        furnitureNameText.text = data.FurnitureName;

        currentLevelText.text = $"{_currentFurniture.CurrentLevel}";

        nextLevelText.text = $"{_currentFurniture.CurrentLevel + 1}";

        costText.text = nextLevel.UpgradeCost.ToString();

        RefreshCoin();

        RefreshUpgradeButton();
    }

    private void RefreshCoin()
    {
        if (Inventory.Instance == null)
        {
            currentCoinText.text = "0";
            return;
        }

        currentCoinText.text = Inventory.Instance.GetItemCount(ItemTypes.Coin).ToString();
    }

    private void RefreshUpgradeButton()
    {
        if (upgradeButton == null)
            return;

        if (_currentFurniture == null || _currentFurniture.NextLevelData == null || Inventory.Instance == null)
        {
            upgradeButton.interactable = false;
            return;
        }

        int cost = _currentFurniture.NextLevelData.UpgradeCost;

        int coins = Inventory.Instance.GetItemCount(ItemTypes.Coin);

        upgradeButton.interactable = coins >= cost;
    }

    private void OnInventoryChanged(ItemTypes itemType)
    {
        if (itemType != ItemTypes.Coin)
            return;

        if (_currentFurniture == null)
            return;

        RefreshCoin();
        RefreshUpgradeButton();
    }

    private void OnUpgradeClicked()
    {
        if (_currentFurniture == null)
            return;

        bool upgraded =
            _currentFurniture.TryUpgrade();

        if (upgraded)
        {
            Close();
        }
        else
        {
            RefreshUI();
        }
    }

    public void Close()
    {
        _currentFurniture = null;

        if (panel != null)
            panel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.RemoveListener(OnUpgradeClicked);

        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);

        if (Instance == this)
            Instance = null;
    }
}