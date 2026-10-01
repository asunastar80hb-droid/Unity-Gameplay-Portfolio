using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Item")]
    [SerializeField] private ItemTypes itemType;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;

    [Header("UI")]
    [SerializeField] private ItemCollection collection;

    public ItemTypes ItemType => itemType;
    public string ItemName => itemName;
    public Sprite Icon => icon;
    public ItemCollection Collection => collection;
}

// sample


// TomatoSeed.asset
//     ItemType   = TomatoSeed
// ItemName   = Tomato Seed
//     Icon       = ...
// Collection = PanelA
//
// Tomato.asset
//     ItemType   = Tomato
// ItemName   = Tomato
// Icon       = ...
// Collection = PanelA
//
// Coin.asset
//     ItemType   = Coin
// ItemName   = Coin
// Icon       = ...
// Collection = PanelA