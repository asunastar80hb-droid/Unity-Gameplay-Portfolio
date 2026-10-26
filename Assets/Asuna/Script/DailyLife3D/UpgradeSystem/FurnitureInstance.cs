using UnityEngine;

public class FurnitureInstance : Interactable
{
    [Header("Furniture")]
    [SerializeField] private FurnitureData furnitureData;

    [SerializeField] private int currentLevel = 1;

    public FurnitureData FurnitureData => furnitureData;
    public int CurrentLevel => currentLevel;
    

    public FurnitureLevelData CurrentLevelData => 
        furnitureData != null
            ? furnitureData.GetLevel(currentLevel)
            : null;

    public FurnitureLevelData NextLevelData =>
              furnitureData != null
            ? furnitureData.GetNextLevel(currentLevel)
            : null;

    public bool CanUpgrade =>
        furnitureData != null &&
        furnitureData.HasNextLevel(currentLevel);

    public override void Interact()
    {
        if (!CanUpgrade)
        {
            Debug.Log("Furniture is already at maximum level.");
            return;
        }

        if (FurnitureUpgradeUI.Instance == null)
        {
            Debug.LogError("FurnitureInstance: FurnitureUpgradeUI is missing.");
            return;
        }

        FurnitureUpgradeUI.Instance.Open(this);
    }

    public bool TryUpgrade()
    {
        if (!CanUpgrade)
            return false;

        FurnitureLevelData nextLevel = NextLevelData;

        if (nextLevel == null)
            return false;

        if (nextLevel.Prefab == null)
        {
            Debug.LogError($"FurnitureInstance: " + $"Prefab for level {nextLevel} is missing.");
            return false;
        }

        if (Inventory.Instance == null)
        {
            Debug.LogError("FurnitureInstance: Inventory is missing.");
            return false;
        }

        if (!Inventory.Instance.HasItem(ItemTypes.Coin, nextLevel.UpgradeCost))
        {
            return false;
        }

        Inventory.Instance.RemoveItem(ItemTypes.Coin, nextLevel.UpgradeCost);

        ReplaceFurniture(nextLevel);

        return true;
    }

    private void ReplaceFurniture(FurnitureLevelData nextLevel)
    {
        Transform parent = transform.parent;

        if (parent == null)
        {
            Debug.LogError("FurnitureInstance: Parent object is missing.");
            return;
        }

        GameObject newFurniture = Instantiate(nextLevel.Prefab, parent);

        newFurniture.transform.localPosition = transform.localPosition;

        // newFurniture.transform.localRotation = transform.localRotation;
        // newFurniture.transform.localScale = transform.localScale;

        if (InteractionManager.Instance != null)
        {
            InteractionManager.Instance.Unregister(this);
        }

        Destroy(gameObject);
    }
}