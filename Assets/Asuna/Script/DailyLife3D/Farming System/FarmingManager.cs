using System.Collections.Generic;
using UnityEngine;

public class FarmingManager : MonoBehaviour
{
    public static FarmingManager Instance { get; private set; }
    
    private readonly List<CropInstance> _crops = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(CropInstance crop)
    {
        if (crop == null)
            return;

        if (_crops.Contains(crop))
            return;

        _crops.Add(crop);
    }
    public void Unregister(CropInstance crop)
    {
        if (crop == null)
            return;

        _crops.Remove(crop);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        for (int i = _crops.Count - 1; i >= 0; i--)
        {
            if (_crops[i] == null)
            {
                _crops.RemoveAt(i);
                continue;
            }

            _crops[i].Tick(deltaTime);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}