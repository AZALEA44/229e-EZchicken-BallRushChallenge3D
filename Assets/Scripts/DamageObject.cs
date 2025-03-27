using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damageAmount = 1; // Amount of damage this object will deal

    

    // This method is called when another collider enters the trigger collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided with us has a PlayerHealth component
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // Call the TakeDamage method to reduce the player's health
            playerHealth.TakeDamage(damageAmount);
            
        }
    }


    
}
