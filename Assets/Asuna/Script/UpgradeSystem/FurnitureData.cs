using UnityEngine;

[CreateAssetMenu(menuName = "Furniture/Furniture Data")]
public class FurnitureData : ScriptableObject
{
    [Header("Furniture")]
    [SerializeField] private string furnitureName;
    [SerializeField] private FurnitureLevelData[] levels;

    public string FurnitureName => furnitureName;
    public FurnitureLevelData[] Levels => levels;

    public FurnitureLevelData GetLevel(int level)
    {
        if (levels == null)
            return null;
        
        int index = level - 1;

        if (index < 0 || index >= levels.Length)
            return null;

        return levels[index];
    }

    public FurnitureLevelData GetNextLevel(int currentLevel)
    {
        return GetLevel(currentLevel + 1);
    }

    public bool HasNextLevel(int currentLevel)
    {
        return currentLevel < levels.Length;
    }
}

// Array Index 0 → Level 1
// Array Index 1 → Level 2
// Array Index 2 → Level 3