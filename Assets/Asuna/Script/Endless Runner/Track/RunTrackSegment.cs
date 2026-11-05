using UnityEngine;

public class RunTrackSegment : MonoBehaviour
{
    [Header("Connection Points")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    public Transform StartPoint => startPoint;
    public Transform EndPoint => endPoint;
}