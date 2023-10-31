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
		Debug.Log("Damage hitbox");
		if(currentGrace > 0) return;
		currentGrace += gracePeriod;
		TakeDamage tdamage = collider.transform.parent.GetComponent<TakeDamage>();
		if (tdamage != null)
		{
			Debug.Log("tdamage");
			tdamage.TriggerTakeDamage(damageAmount);
		}
        
	}
}


