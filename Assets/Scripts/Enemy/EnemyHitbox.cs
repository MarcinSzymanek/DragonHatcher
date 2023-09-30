using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{

    public int damageAmount;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the player character
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colliding with player");
            TakeDamageScript playerDamage = collision.collider.gameObject.GetComponent<TakeDamageScript>();
               
            if (playerDamage != null)
            {
                playerDamage.TakeDamage(damageAmount);
            }
        }
    }
}
