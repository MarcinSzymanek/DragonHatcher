using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EnemyHitbox : MonoBehaviour
{

	public int damageAmount;
	private int currentGrace = 0;
	public int gracePeriod;
	public List<TakeDamage> objDamage;
	public bool destroyOnDamage;
	void Start(){
		objDamage = new List<TakeDamage>();
	}
	
	void FixedUpdate(){
		if(currentGrace > 0){
			currentGrace--;
			return;
		}
		if(objDamage.Count == 0) return;
		bool dealtDamage = false;
		foreach(TakeDamage d in objDamage){
			if(d == null) continue;
			if(objDamage.Count == 0) return;
			d.TriggerTakeDamage(damageAmount);
			dealtDamage = true;}
		
		if(dealtDamage && destroyOnDamage){
			transform.parent.gameObject.SetActive(false);
		}
		
		if(objDamage.Count == 0) return;
		currentGrace = gracePeriod;
	}
    
	private void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("Collided with: " + collider.gameObject.name);
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


