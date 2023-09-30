using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{

    public int damageAmount;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		
        Debug.Log("Colliding with player");
        TakeDamageScript playerDamage = collision.gameObject.GetComponent<TakeDamageScript>();
           
        if (playerDamage != null)
        {
            playerDamage.TakeDamage(damageAmount);
        }
        
    }
}
