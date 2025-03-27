using TMPro;
using UnityEngine;
using System.Collections;

public class CoinCollector : MonoBehaviour
{
    private int Coin = 0;

    public TextMeshProUGUI coinText;
    public ParticleSystem pointParticlesPrefab;
    public Transform pointParticleSpawnPoint;
    public Light coinLight; // Yellow light effect
    public Transform coinLightPoint; // Point where the yellow light appears
    private float originalLightIntensity;

    // Sound Effect
    public AudioClip coinSound; // Assign this in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        if (coinLight != null)
        {
            originalLightIntensity = coinLight.intensity;
            coinLight.enabled = false; // Ensure it's off initially
        }

        // Get or add an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin")) // Make sure the coin is tagged as "Coin"
        {
            Coin++;
            coinText.text = "Point: " + Coin.ToString();
            Debug.Log(Coin);

            // Play coin sound
            if (coinSound != null)
            {
                audioSource.PlayOneShot(coinSound);
            }

            Destroy(other.gameObject); // Destroy the coin
            PlayPointParticles();
            StartCoroutine(AnimateTextSize()); // Start text size animation
            StartCoroutine(FlashYellowLight());
        }
    }

    void PlayPointParticles()
    {
        if (pointParticlesPrefab != null && pointParticleSpawnPoint != null)
        {
            ParticleSystem particles = Instantiate(pointParticlesPrefab, pointParticleSpawnPoint.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration); // Destroy after effect ends
        }
    }

    IEnumerator AnimateTextSize()
    {
        float duration = 0.15f; // Duration of animation
        float maxSize = 1.5f; // Scale multiplier
        float originalSize = coinText.fontSize;

        // Increase size
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            coinText.fontSize = Mathf.Lerp(originalSize, originalSize * maxSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset size
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            coinText.fontSize = Mathf.Lerp(originalSize * maxSize, originalSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        coinText.fontSize = originalSize; // Ensure it returns to normal
    }

    private IEnumerator FlashYellowLight()
    {
        if (coinLight != null && coinLightPoint != null)
        {
            coinLight.transform.position = coinLightPoint.position;
            coinLight.enabled = true;
            coinLight.color = Color.yellow;
            coinLight.intensity = originalLightIntensity * 2; // Increase intensity
            yield return new WaitForSeconds(0.5f);
            coinLight.intensity = originalLightIntensity;
            coinLight.enabled = false;
        }
    }
}
