using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Drag your player here
    public float smoothSpeed = 0.125f;
    public Vector3 offset;        // Adjust in Inspector (e.g., 0, 5, -10)

void Start()
{
    // This automatically grabs the distance between the camera 
    // and player at the moment the game starts.
    offset = transform.position - target.position;
}
    void LateUpdate()
    {
        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;
        
        // Smoothly interpolate between current position and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Apply to the camera
        transform.position = smoothedPosition;

        // Optional: Always look at the player
        // transform.LookAt(target);
    }
}