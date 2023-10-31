using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EnemyHitbox : MonoBehaviour
{

	public int damageAmount;
	private int currentGrace = 0;
	public int gracePeriod;
	public List<TakeDamage> objDamage;
	
	void Start(){
		objDamage = new List<TakeDamage>();
	}
	
	void FixedUpdate(){
		if(currentGrace > 0){
			currentGrace--;
			return;
		}
		if(objDamage.Count == 0) return;
		
		foreach(TakeDamage d in objDamage){
			d.TriggerTakeDamage(damageAmount);
		}
		currentGrace = gracePeriod;
	}
    
	private void OnTriggerEnter2D(Collider2D collider){
		TakeDamage tdamage = collider.transform.parent.GetComponent<TakeDamage>();
		if(objDamage.Contains(tdamage)) return;
		objDamage.Add(tdamage);
	}
	
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerExit2D(Collider2D collider)
	{
		TakeDamage tdamage = collider.transform.parent.GetComponent<TakeDamage>();
		objDamage.Remove(tdamage);
	}
}


