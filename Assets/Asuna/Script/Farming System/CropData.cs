using UnityEngine;

[CreateAssetMenu(menuName = "Farming/Crop Data")]
public class CropData : ScriptableObject
{
    [Header("Crop")]
    [SerializeField] private PlantType plantType;
    [SerializeField] private string cropName;

    [Header("Items")]
    [SerializeField] private ItemTypes seedItem;
    [SerializeField] private ItemTypes harvestItem;
    [SerializeField] private int harvestAmount;

    [Header("Growth")]
    [SerializeField] private GameObject[] growthStages;
    [SerializeField] private float growTime;

    public PlantType PlantType => plantType;
    public string CropName => cropName;

    public ItemTypes SeedItem => seedItem;
    public ItemTypes HarvestItem => harvestItem;
    public int HarvestAmount => harvestAmount;

    public GameObject[] GrowthStages => growthStages;
    public float GrowTime => growTime;
}


//// sample

// TomatoCropData
//     PlantType    = Tomato
// SeedItem     = TomatoSeed
// HarvestItem  = Tomato
// HarvestAmount = 6
// GrowTime      = 60
//
// CarrotCropData
//     PlantType    = Carrot
// SeedItem     = CarrotSeed
// HarvestItem  = Carrot
// HarvestAmount = 4
// GrowTime      = 45
//
// PepperCropData
//     PlantType    = Piper
// SeedItem     = PepperSeed
// HarvestItem  = Pepper
// HarvestAmount = 5
// GrowTime      = 75

 