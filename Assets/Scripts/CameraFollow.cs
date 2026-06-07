using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public float smoothSpeed = 0.125f;

    [Header("Camera Bounds")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("Aspect Ratio")]
    public float targetAspect = 16f / 9f;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        UpdateAspect();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, -10f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        float camVertExtent = cam.orthographicSize;
        float camHorzExtent = cam.aspect * camVertExtent;

        float minX = minBounds.x + camHorzExtent;
        float maxX = maxBounds.x - camHorzExtent;
        float minY = minBounds.y + camVertExtent;
        float maxY = maxBounds.y - camVertExtent;

        float clampedX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, smoothedPosition.z);

        UpdateAspect();
    }

    void UpdateAspect()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Rect rect = new Rect(0f, (1f - scaleHeight) / 2f, 1f, scaleHeight);
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1f / scaleHeight;
            Rect rect = new Rect((1f - scaleWidth) / 2f, 0f, scaleWidth, 1f);
            cam.rect = rect;
        }
    }
}
