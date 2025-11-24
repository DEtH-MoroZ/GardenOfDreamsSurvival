using UnityEngine;

public class SmoothFollowCamera2D : MonoBehaviour
{
    [Header("Follow Targets")]
    public Transform target;
    public Transform targetToAim;

    [Header("Camera Settings")]
    public float smoothTime = 0.3f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float zoomPadding = 2f;

    private Camera cam;
    private Vector3 velocity = Vector3.zero;
    private float zoomVelocity = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = CalculateTargetPosition();
        float targetZoom = CalculateTargetZoom();

        // Smoothly move camera
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Smoothly zoom camera
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothTime);
    }

    Vector3 CalculateTargetPosition()
    {
        Vector3 center = target.position;

        if (targetToAim != null)
        {
            // Calculate midpoint between two targets
            center = (target.position + targetToAim.position) / 2f;
        }

        // Keep camera's original Z position
        center.z = transform.position.z;
        return center;
    }

    float CalculateTargetZoom()
    {
        if (targetToAim == null)
        {
            return minZoom;
        }

        // Calculate required zoom based on distance between targets
        float distance = Vector3.Distance(target.position, targetToAim.position);

        // Adjust zoom based on screen aspect ratio
        float requiredZoom = (distance + zoomPadding) / (2f * cam.aspect);

        return Mathf.Clamp(requiredZoom, minZoom, maxZoom);
    }

    // Optional: Visualize camera bounds in Scene view
    void OnDrawGizmosSelected()
    {
        if (cam == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(
            cam.orthographicSize * 2 * cam.aspect,
            cam.orthographicSize * 2,
            0
        ));
    }
}