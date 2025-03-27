using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    public SpeedRunTimer timer;
    public ParticleSystem finishParticlesPrefab;
    public Transform finishParticleSpawnPoint;
    public AudioClip winSound;
    private AudioSource audioSource;
    public Restart restartScript;  // Reference to the Restart script

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer.StopTimer();
            Debug.Log("You Won!!!");
            PlayFinishParticles();
            PlayWinSound();
            ShowCursor(); // Show cursor when player wins
            restartScript.PlayerDied(); // Show the buttons when the player wins
        }
    }

    void PlayFinishParticles()
    {
        if (finishParticlesPrefab != null && finishParticleSpawnPoint != null)
        {
            Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
            ParticleSystem particles = Instantiate(finishParticlesPrefab, finishParticleSpawnPoint.position, rotation);
            particles.Play();
        }
    }

    void PlayWinSound()
    {
        if (winSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(winSound);
        }
    }

    void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true; // Makes the cursor visible
    }
}
