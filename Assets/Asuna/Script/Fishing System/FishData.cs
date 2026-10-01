using UnityEngine;

[CreateAssetMenu(menuName = "Fishing/Fish Data")]
public class FishData : ScriptableObject
{
    [Header("Fish")]
    [SerializeField] private string fishName;
    [SerializeField] private Sprite icon;

    [Header("Inventory")]
    [SerializeField] private ItemData fishItem;

    [Header("Gameplay")]
    [SerializeField] private float difficultyMultiplier = 1f;

    public string FishName => fishName;
    public Sprite Icon => icon;
    public ItemData FishItem => fishItem;
    public float DifficultyMultiplier => difficultyMultiplier;
}