using System.Collections;
using UnityEngine;
using static UnityEngine.Color;

public class ThrowThread : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] private Transform wood;
    void Start()
    { 
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = true;
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
        _lineRenderer.startColor = gray;
        _lineRenderer.endColor = gray;
        _lineRenderer.SetPosition(0 , transform.position);
        _lineRenderer.SetPosition(1 , wood.position);
    }
}
