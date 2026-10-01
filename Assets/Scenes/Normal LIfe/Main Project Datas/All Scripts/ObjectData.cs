using UnityEngine;

public class ObjectData : MonoBehaviour
{
    [SerializeField] private PlantType plantType;

    public PlantType PlantType => plantType;
}