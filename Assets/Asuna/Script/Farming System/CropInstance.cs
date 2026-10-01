using UnityEngine;

public class CropInstance
{
    private readonly CropData _data;
    private readonly Transform _place;

    private GameObject _currentVisual;

    private float _growTimer;
    private int _stageIndex;

    public CropData Data => _data;

    public bool IsReady =>
        _data != null &&
        _data.GrowthStages != null &&
        _data.GrowthStages.Length > 0 &&
        _stageIndex >= _data.GrowthStages.Length - 1;

    public CropInstance(CropData data, Transform place)
    {
        _data = data;
        _place = place;

        if (_data == null)
        {
            Debug.LogError("CropInstance: CropData is null.");
            return;
        }

        if (_place == null)
        {
            Debug.LogError("CropInstance: Farm place is null.");
            return;
        }

        SpawnStage(0);
    }

    public void Tick(float deltaTime)
    {
        if (_data == null || IsReady)
            return;

        if (_data.GrowthStages == null || _data.GrowthStages.Length == 0)
            return;

        if (_data.GrowTime <= 0f)
        {
            SetStage(_data.GrowthStages.Length - 1);
            return;
        }

        _growTimer += deltaTime;

        float stageDuration = _data.GrowTime / _data.GrowthStages.Length;

        int newStage = Mathf.FloorToInt(_growTimer / stageDuration);

        newStage = Mathf.Clamp(newStage, 0, _data.GrowthStages.Length - 1);

        if (newStage != _stageIndex)
            SetStage(newStage);
    }

    private void SpawnStage(int index)
    {
        if (_data.GrowthStages == null || _data.GrowthStages.Length == 0)
            return;

        SetStage(index);
    }

    private void SetStage(int index)
    {
        if (_currentVisual != null)
            Object.Destroy(_currentVisual);

        _stageIndex = index;

        GameObject prefab = _data.GrowthStages[_stageIndex];

        if (prefab == null)
            return;

        _currentVisual = Object.Instantiate(prefab, _place);

        _currentVisual.transform.localPosition =
            Vector3.zero;
    }

    public GameObject Harvest()
    {
        if (!IsReady)
            return null;

        GameObject harvestedVisual = _currentVisual;

        _currentVisual = null;

        return harvestedVisual;
    }
}