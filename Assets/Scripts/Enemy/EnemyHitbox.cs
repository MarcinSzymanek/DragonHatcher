using UnityEngine;
using UnityEditor;

public class EnemyHitbox : MonoBehaviour
{

	public int damageAmount;
	private int currentGrace = 0;
	public int gracePeriod;
	
	void FixedUpdate(){
		if(currentGrace > 0){
			currentGrace--;
		}
	}
    
	private void OnTriggerStay2D(Collider2D collider){
		if(currentGrace > 0) return;
		currentGrace += gracePeriod;
		TakeDamage tdamage = collider.transform.parent.GetComponent<TakeDamage>();
		if (tdamage != null)
		{
			tdamage.TakeDamageOnHit(damageAmount);
		}
        
	}
}


