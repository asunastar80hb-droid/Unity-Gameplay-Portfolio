using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RunTrackEntry : MonoBehaviour
{
    [SerializeField] private string runnerTag = "Player";

    private RunTrackSegment segment;

    private void Awake()
    {
        segment = GetComponentInParent<RunTrackSegment>();
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(runnerTag))
            return;

        if (segment == null || RunTrackManager.Instance == null)
            return;

        RunTrackManager.Instance.OnPlayerEnteredTile(segment);
    }
}