using System.Collections.Generic;
using UnityEngine;

public class FarmPlot : Interactable
{
    private readonly List<Transform> _farmPlaces = new();
    private readonly List<CropInstance> _currentCrops = new();
    
    public IReadOnlyList<CropInstance> CurrentCrops => _currentCrops;
    public bool IsPlanted => _currentCrops.Count > 0;
    public bool IsHarvestable => IsPlanted && AreAllCropsReady();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform place = transform.GetChild(i);

            if (place != null)
                _farmPlaces.Add(place);
        }
    }
    public override void Interact()
    {
        if (IsHarvestable)
        {
            Harvest();
            return;
        }

        if (IsPlanted)
            return;

        if (FarmingUIManager.Instance == null)
        {
            Debug.LogError("FarmPlot: FarmingUIManager is missing.");
            return;
        }

        FarmingUIManager.Instance.Open(this);
    }

    public bool TryPlant(CropData cropData)
    {
        if (!CanPlant(cropData))
            return false;

        _currentCrops.Clear();

        foreach (Transform farmPlace in _farmPlaces)
        {
            if (farmPlace == null)
                continue;

            CropInstance crop = new CropInstance(cropData, farmPlace);

            _currentCrops.Add(crop);

            FarmingManager.Instance.Register(crop);
        }

        return _currentCrops.Count > 0;
    }

    public bool CanPlant(CropData cropData)
    {
        if (cropData == null)
            return false;

        if (IsPlanted)
            return false;

        if (FarmingManager.Instance == null)
            return false;

        if (_farmPlaces.Count == 0)
            return false;

        if (cropData.GrowthStages == null || cropData.GrowthStages.Length == 0)
            return false;

        return true;
    }

    private bool AreAllCropsReady()
    {
        if (_currentCrops.Count == 0)
            return false;

        foreach (CropInstance crop in _currentCrops)
        {
            if (crop == null || !crop.IsReady)
                return false;
        }

        return true;
    }

    public void Harvest()
    {
        if (!IsHarvestable)
            return;

        CropData cropData = _currentCrops[0].Data;

        foreach (CropInstance crop in _currentCrops)
        {
            if (crop == null)
                continue;

            GameObject visual = crop.Harvest();

            if (visual != null)
                Destroy(visual);

            FarmingManager.Instance.Unregister(crop);
        }

        _currentCrops.Clear();

        if (Inventory.Instance != null)
        {
            Inventory.Instance.AddItem(cropData.HarvestItem, cropData.HarvestAmount);
        }
    }
}