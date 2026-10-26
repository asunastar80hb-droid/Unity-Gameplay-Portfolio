using UnityEngine;

[System.Serializable]
public class FurnitureLevelData
{
    // [Header("Level")]
    // [SerializeField] private int level;

    [Header("Visual")]
    [SerializeField] private GameObject prefab;

    [Header("Upgrade")]
    [SerializeField] private int upgradeCost;
    //
    // public int Level => level;
    public GameObject Prefab => prefab;
    public int UpgradeCost => upgradeCost;
}