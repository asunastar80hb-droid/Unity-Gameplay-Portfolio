using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    private float _speed = 80;
    void Update()
    {
        transform.Rotate(0,0, Time.deltaTime * _speed);
    }
}
