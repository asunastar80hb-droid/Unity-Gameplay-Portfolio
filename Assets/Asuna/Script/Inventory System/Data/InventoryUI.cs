using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ItemCardUI cardPrefab;
    [SerializeField] private Transform[] collectionParents;

    private readonly Dictionary<ItemTypes, ItemCardUI> _cards = new();

    private void OnEnable()
    {
        if (Inventory.Instance == null)
            return;

        Inventory.Instance.OnItemChanged += HandleItemChanged;

        RefreshAll();
    }

    private void OnDisable()
    {
        if (Inventory.Instance == null)
            return;

        Inventory.Instance.OnItemChanged -= HandleItemChanged;
    }

    private void HandleItemChanged(ItemTypes itemType)
    {
        int amount =
            Inventory.Instance.GetItemCount(itemType);

        if (amount <= 0)
        {
            RemoveCard(itemType);
        }
        else if (_cards.TryGetValue(itemType, out ItemCardUI card))
        {
            InventoryItem item =
                Inventory.Instance.GetItem(itemType);

            card.Setup(item);
        }
        else
        {
            CreateCard(itemType);
        }

        RefreshCollection(itemType);
    }

    private void RefreshAll()
    {
        ClearCards();

        foreach (InventoryItem item in Inventory.Instance.GetItems())
        {
            CreateCard(item.ItemType);
        }

        foreach (Transform parent in collectionParents)
        {
            if (parent == null)
                continue;

            BagCount bagCount =
                parent.GetComponent<BagCount>();

            if (bagCount != null)
                bagCount.Refresh();
        }
    }

    private void CreateCard(ItemTypes itemType)
    {
        if (_cards.ContainsKey(itemType))
            return;

        InventoryItem item =
            Inventory.Instance.GetItem(itemType);

        if (item == null)
            return;

        ItemData data = item.Data;

        Transform parent =
            GetCollectionParent(data.Collection);

        if (parent == null)
            return;

        if (cardPrefab == null)
        {
            Debug.LogError(
                "InventoryUI: Card Prefab is missing."
            );

            return;
        }

        ItemCardUI card =
            Instantiate(cardPrefab, parent);

        card.Setup(item);

        _cards.Add(itemType, card);
    }

    private void RemoveCard(ItemTypes itemType)
    {
        if (!_cards.TryGetValue(
                itemType,
                out ItemCardUI card))
        {
            return;
        }

        if (card != null)
            Destroy(card.gameObject);

        _cards.Remove(itemType);
    }

    private void RefreshCollection(ItemTypes itemType)
    {
        ItemData data =
            Inventory.Instance.GetItemData(itemType);

        if (data == null)
            return;

        Transform parent =
            GetCollectionParent(data.Collection);

        if (parent == null)
            return;

        BagCount bagCount =
            parent.GetComponent<BagCount>();

        if (bagCount != null)
            bagCount.Refresh();
    }

    private Transform GetCollectionParent(
        ItemCollection collection)
    {
        int index = (int)collection - 1;

        if (index < 0 ||
            index >= collectionParents.Length)
        {
            Debug.LogError(
                $"InventoryUI: No parent assigned for {collection}."
            );

            return null;
        }

        return collectionParents[index];
    }

    private void ClearCards()
    {
        foreach (ItemCardUI card in _cards.Values)
        {
            if (card != null)
                Destroy(card.gameObject);
        }

        _cards.Clear();
    }
}