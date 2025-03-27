using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 15f; // Adjust as needed
    public AudioClip jumpSound; // Assign this in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Get or add an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched the jump pad: " + other.name);

        // Try to get Rigidbody on the object or its parent
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();

        if (rb != null)
        {
            Debug.Log("Rigidbody found on: " + rb.gameObject.name);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset Y velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Play jump sound
            if (jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
        else
        {
            Debug.LogError(other.name + " has NO Rigidbody in itself or its parent!");
        }
    }
}
