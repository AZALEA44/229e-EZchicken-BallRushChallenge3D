using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float distance = 5f; // Distance behind the player
    public float height = 2f; // Height above the player
    public float rotationSpeed = 2f; // Mouse sensitivity

    private float yaw = 0f; // Horizontal rotation
    private float pitch = 15f; // Vertical rotation

    private Vector3 shakeOffset = Vector3.zero;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;

    void LateUpdate()
    {
        if (player == null) return;

        // Get mouse input
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -15f, 60f); // Limit vertical angle

        // Calculate camera position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, height, -distance);
        Vector3 targetPosition = player.position + offset;

        // Apply screen shake
        if (shakeDuration > 0)
        {
            shakeOffset = new Vector3(
                Random.Range(-shakeMagnitude, shakeMagnitude),
                Random.Range(-shakeMagnitude, shakeMagnitude),
                Random.Range(-shakeMagnitude, shakeMagnitude)
            );

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }

        // Smoothly move camera to position with shake
        transform.position = Vector3.Lerp(transform.position, targetPosition + shakeOffset, Time.deltaTime * 10f);
        transform.LookAt(player.position);
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
