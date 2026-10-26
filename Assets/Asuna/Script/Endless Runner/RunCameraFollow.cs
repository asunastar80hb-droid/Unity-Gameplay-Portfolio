using UnityEngine;

public class RunCameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Position")]
    [SerializeField] private float height = 5f;
    [SerializeField] private float distance = 7f;

    [Header("Camera Angle")]
    [SerializeField] private float lookAhead = 5f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Position the camera behind and above the runner.
        Vector3 cameraPosition =
            target.position
            - target.forward * distance
            + Vector3.up * height;

        transform.position = cameraPosition;

        // Look at a point ahead of and slightly above the runner.
        Vector3 lookPoint =
            target.position
            + target.forward * lookAhead
            + Vector3.up * 1.5f;

        transform.LookAt(lookPoint);
    }
}