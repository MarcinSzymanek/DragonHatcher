using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ContactDamage : MonoBehaviour
{

	public int damageAmount;
	private int currentGrace = 0;
	public int gracePeriod;
	// This keeps track of which objects should be damaged
	public List<TakeDamage> objDamage;
	
	// This keeps track of which objects have already been damaged
	public List<TakeDamage> objDamageDealt;
	
	public bool destroyOnDamage;
	public bool logCollisions;
	
	[field: SerializeField]
	private bool dealDamageOnce_ = true;
	
	private string parentName_;
	public event System.Action<Rigidbody2D> damageEffectEvent;
	void Start(){
		objDamage = new List<TakeDamage>();
		objDamageDealt = new List<TakeDamage>();
		parentName_ = transform.parent.name;
	}
	
	void FixedUpdate(){
		//if(currentGrace > 0){
		//	currentGrace--;
		//	return;
		//}
		if(objDamage.Count == 0) return;
		bool dealtDamage = false;
		List<int> toRemove = new List<int>();
		
		for(int i = 0; i < objDamage.Count; i++){			
			var d = objDamage[i];
			if(d == null) continue;
			if(dealDamageOnce_ && objDamageDealt.Contains(d)) continue;
			d.TriggerTakeDamage(damageAmount);
			if(dealDamageOnce_) objDamageDealt.Add(d);
			dealtDamage = true;
			if(d.dead) toRemove.Add(i);
		}
		
		foreach(int i in toRemove){
			if(i < objDamage.Count) objDamage.RemoveAt(i);
		}
		// Debug.Log("Done");
		if(dealtDamage && destroyOnDamage){
			var ctrl = transform.parent.GetComponent<ParticleController>();
			if(ctrl != null) ctrl.DetachParticles();
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
		damageEffectEvent?.Invoke(collider.transform.parent.GetComponent<Rigidbody2D>());
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


