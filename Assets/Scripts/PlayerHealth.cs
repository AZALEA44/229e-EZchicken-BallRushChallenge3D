using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public TextMeshProUGUI healthText;
    public ParticleSystem damageParticlesPrefab;
    public Transform damageParticleSpawnPoint;
    public Renderer playerRenderer;
    public Color damageColor = Color.black;
    private Color originalColor;
    private Color originalTextColor;
    private Vector3 originalTextPosition;
    public Light damageLight;
    public Transform damageLightPoint;
    private float originalLightIntensity;
    public AudioClip deathSound;

    private ThirdPersonCamera cameraController;
    public GameObject pauseButton;

    private bool isPaused = false;
    private bool isDead = false;

    public TextMeshProUGUI deadText;

    // Sound Effects
    public AudioClip damageSound; // Assign in Inspector
    public AudioClip healSound;   // Assign in Inspector
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        if (healthText != null)
        {
            originalTextPosition = healthText.rectTransform.localPosition;
            originalTextColor = healthText.color;
        }

        if (damageLight != null)
        {
            originalLightIntensity = damageLight.intensity;
            damageLight.enabled = false;
        }

        cameraController = FindObjectOfType<ThirdPersonCamera>();

        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }

        if (deadText != null)
        {
            deadText.gameObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get or add an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isDead)
        {
            TogglePause();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        PlayDamageParticles();
        StartCoroutine(ChangeColorTemporarily());
        StartCoroutine(ShakeText());
        StartCoroutine(ChangeTextColor());
        StartCoroutine(FlashRedLight());

        cameraController?.ShakeCamera(1f, 2f);

        // Play damage sound
        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        UpdateHealthUI();
    }

    public void IncreaseHealth(int healthToAdd)
    {
        currentHealth = Mathf.Min(currentHealth + healthToAdd, maxHealth);
        StartCoroutine(ChangeTextColorOnHeal());

        // Play heal sound
        if (healSound != null)
        {
            audioSource.PlayOneShot(healSound);
        }

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        isDead = true;

        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (deadText != null)
        {
            deadText.gameObject.SetActive(true);
        }

        FindObjectOfType<Restart>().PlayerDied();
    }

    void PlayDamageParticles()
    {
        if (damageParticlesPrefab != null && damageParticleSpawnPoint != null)
        {
            ParticleSystem particles = Instantiate(damageParticlesPrefab, damageParticleSpawnPoint.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }
    }

    private IEnumerator ChangeColorTemporarily()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = damageColor;
            yield return new WaitForSeconds(1f);
            playerRenderer.material.color = originalColor;
        }
    }

    private IEnumerator ShakeText()
    {
        float duration = 1.1f;
        float magnitude = 20f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float xOffset = Random.Range(-magnitude, magnitude);
            float yOffset = Random.Range(-magnitude, magnitude);
            healthText.rectTransform.localPosition = originalTextPosition + new Vector3(xOffset, yOffset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        healthText.rectTransform.localPosition = originalTextPosition;
    }

    private IEnumerator ChangeTextColor()
    {
        if (healthText != null)
        {
            healthText.color = Color.black;
            yield return new WaitForSeconds(1f);
            healthText.color = originalTextColor;
        }
    }

    private IEnumerator ChangeTextColorOnHeal()
    {
        if (healthText != null)
        {
            healthText.color = Color.green;
            yield return new WaitForSeconds(1f);
            healthText.color = originalTextColor;
        }
    }

    private IEnumerator FlashRedLight()
    {
        if (damageLight != null && damageLightPoint != null)
        {
            damageLight.transform.position = damageLightPoint.position;
            damageLight.enabled = true;
            damageLight.color = Color.red;
            damageLight.intensity = originalLightIntensity * 2;
            yield return new WaitForSeconds(0.5f);
            damageLight.intensity = originalLightIntensity;
            damageLight.enabled = false;
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
