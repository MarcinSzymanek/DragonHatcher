using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ContactDamage : MonoBehaviour
{

	public int damageAmount;
	private int currentGrace = 0;
	public int gracePeriod;
	public List<TakeDamage> objDamage;
	public bool destroyOnDamage;
	public bool logCollisions;
	private string parentName_;
	void Start(){
		objDamage = new List<TakeDamage>();
		parentName_ = transform.parent.name;
	}
	
	void FixedUpdate(){
		if(currentGrace > 0){
			currentGrace--;
			return;
		}
		if(objDamage.Count == 0) return;
		bool dealtDamage = false;
		List<int> toRemove = new List<int>();
		// Debug.Log("Iterating through objDamage list");
		for(int i = 0; i < objDamage.Count; i++){
			var d = objDamage[i];
			if(d == null) continue;
			d.TriggerTakeDamage(damageAmount);
			dealtDamage = true;
			if(d.dead) toRemove.Add(i);
		}
		
		foreach(int i in toRemove){
			if(i < objDamage.Count) objDamage.RemoveAt(i);
		}
		// Debug.Log("Done");
		if(dealtDamage && destroyOnDamage){
			transform.parent.gameObject.SetActive(false);
		}
		
		if(objDamage.Count == 0) return;
		currentGrace = gracePeriod;
	}
    
	private void OnTriggerEnter2D(Collider2D collider){
		// Debug.Log("Collided with: " + collider.gameObject.name);
		TakeDamage tdamage;
		try {
			if(logCollisions) Debug.Log(parentName_ + " collided with " + collider.transform.parent.name);
			if(!collider || !collider.transform.parent.TryGetComponent<TakeDamage>(out tdamage)) return;
		}
			catch(UnityException e){
				Debug.LogWarning("Exception caught: ");
				Debug.LogError(e.Message);
				Debug.LogError(e.Source.ToString());
				return;
		}
		
		if(objDamage.Contains(tdamage)) return;
		objDamage.Add(tdamage);
	}
	
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerExit2D(Collider2D collider)
	{
		TakeDamage tdamage;
		if(!collider || !collider.transform.parent.TryGetComponent<TakeDamage>(out tdamage)) return;
		if (tdamage.dead) return;
		objDamage.Remove(tdamage);
	}
}


