using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _mainCamera;
    
    private void Awake()
    {
        _mainCamera = Camera.main.transform;
        transform.SetParent(null);
        transform.LookAt(_mainCamera);
    }
}