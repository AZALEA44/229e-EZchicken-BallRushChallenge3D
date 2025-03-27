using UnityEngine;
using System.Collections;


public class HealthPickup : MonoBehaviour
{
    public ParticleSystem HpParticlesPrefab;
    public Transform HpParticleSpawnPoint;


    public int healthBonus = 1; // Amount of health to restore
    private PlayerHealth playerHealth; // Reference to PlayerHealth script
    

    void Start()
    {
        // Try to get the PlayerHealth script on the player
        playerHealth = FindObjectOfType<PlayerHealth>();

       
    }

    void PlayPointParticles()
    {
        if (HpParticlesPrefab != null && HpParticleSpawnPoint != null)
        {
            ParticleSystem particles = Instantiate(HpParticlesPrefab, HpParticleSpawnPoint.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration); // Destroy after effect ends
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object collided with the player (the player's tag must be "Player")
        if (other.CompareTag("Player"))
        {
            // Call the method to increase health
            playerHealth.IncreaseHealth(healthBonus);

            PlayPointParticles();
            

            // Optionally, destroy the pickup object after use
            Destroy(gameObject);
        }
    }

   
}
