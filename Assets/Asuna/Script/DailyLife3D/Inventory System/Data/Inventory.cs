using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Header("Item Definitions")]
    [SerializeField] private ItemData[] availableItems;

    [Header("Initial Items")]
    [SerializeField] private InitialItem[] initialItems;

    private readonly Dictionary<ItemTypes, ItemData> _itemDefinitions = new();
    private readonly Dictionary<ItemTypes, InventoryItem> _items = new();

    public event Action<ItemTypes> OnItemChanged;

    [Serializable]
    private struct InitialItem
    {
        public ItemTypes itemType;
        public int amount;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeDefinitions();
        InitializeInventory();
    }

    private void InitializeDefinitions()
    {
        _itemDefinitions.Clear();

        foreach (ItemData itemData in availableItems)
        {
            if (itemData == null)
                continue;

            if (_itemDefinitions.ContainsKey(itemData.ItemType))
            {
                Debug.LogWarning($"Inventory: Duplicate ItemType found: {itemData.ItemType}");
                continue;
            }

            _itemDefinitions.Add(itemData.ItemType, itemData);
        }
    }
    private void InitializeInventory()
    {
        _items.Clear();

        foreach (InitialItem initialItem in initialItems)
        {
            if (initialItem.amount <= 0)
                continue;

            AddItem(initialItem.itemType, initialItem.amount);
        }
    }

    public void AddItem(ItemTypes itemType, int amount)
    {
        if (amount <= 0)
            return;

        if (!_itemDefinitions.TryGetValue(itemType, out ItemData data))
        {
            Debug.LogError($"Inventory: No ItemData found for {itemType}.");
            return;
        }

        if (_items.TryGetValue(itemType, out InventoryItem item))
        {
            item.Add(amount);
        }
        else
        {
            _items.Add(itemType, new InventoryItem(data, amount));
        }

        OnItemChanged?.Invoke(itemType);
    }

    public bool RemoveItem(ItemTypes itemType, int amount)
    {
        if (amount <= 0)
            return false;

        if (!_items.TryGetValue(itemType, out InventoryItem item))
            return false;

        if (!item.Remove(amount))
            return false;

        if (item.Amount <= 0)
            _items.Remove(itemType);

        OnItemChanged?.Invoke(itemType);
        return true;
    }

    public bool HasItem(ItemTypes itemType, int amount)
    {
        if (amount <= 0)
            return true;

        return _items.TryGetValue(itemType, out InventoryItem item) && item.Amount >= amount;
    }
    public int GetItemCount(ItemTypes itemType)
    {
        return _items.TryGetValue(itemType, out InventoryItem item) ? item.Amount : 0;
    }

    public ItemData GetItemData(ItemTypes itemType)
    {
        return _itemDefinitions.TryGetValue(itemType, out ItemData data) ? data : null;
    }

    public IReadOnlyCollection<InventoryItem> GetItems()
    {
        return _items.Values;
    }
    
    public InventoryItem GetItem(ItemTypes itemType)
    {
        return _items.TryGetValue(itemType, out InventoryItem item) ? item : null;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}

//      ItemData
//          ├── ItemType
//          ├── Name
//          ├── Icon
//          └── Collection
//     
//     InventoryItem
//         └── Amount
//
//     ItemCardUI
//         ├──  Icon
//         └──  Amount