using UnityEngine;

public class RotationHalf : MonoBehaviour
{
    public float rotationAngle = 45f; // Maximum angle to rotate
    public float speed = 2f; // Speed of rotation

    private float startRotation;
    private float direction = 1;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        float angle = startRotation + Mathf.Sin(Time.time * speed) * rotationAngle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
