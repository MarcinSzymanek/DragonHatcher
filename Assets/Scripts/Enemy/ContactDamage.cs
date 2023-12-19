using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


// Cooldown for damage is in FRAMES per fixed update!
public class ContactDamage : MonoBehaviour
{
	[System.Serializable]
	public class DamageToken{
		public DamageToken(TakeDamage dmg, float baseCd, int applyDamage = 0){
			dmgScript = dmg;
			baseDmgCooldown = baseCd;
			
			damage = applyDamage;
			if(applyDamage > 0){
				DealDamage();
			}
		}
		public void DealDamage(){
			if(cooldownRemaining > 1) return;
			dmgScript.TriggerTakeDamage(damage);
			cooldownRemaining = baseDmgCooldown;
		}
		
		public bool Contains(TakeDamage dmg){
			return dmgScript == dmg;
		}
		
		public void ProcessCooldown(){
			cooldownRemaining -= 1;
			if(cooldownRemaining < 1){
				DealDamage();
			}
		}
		int damage;
		float baseDmgCooldown;
		public float cooldownRemaining;
		public TakeDamage dmgScript;
		public bool effectApplied = false;
	}
	
	public int damageAmount;
	private int currentGrace = 0;
	public int gracePeriod;
	
	public List<DamageToken> dmgTokens;
	// This keeps track of which objects should be damaged
	public List<TakeDamage> objDamage;
	
	// This keeps track of which objects have already been damaged
	public List<TakeDamage> objDamageDealt;
	
	public bool destroyOnDamage;
	public bool logCollisions;
	
	[field: SerializeField]
	private bool dealDamageOnce_ = true;
	private bool dealtDamage = false;
	
	private string parentName_;
	public event System.Action<Rigidbody2D> damageEffectEvent;
	void Start(){
		dmgTokens = new List<DamageToken>();
		objDamage = new List<TakeDamage>();
		objDamageDealt = new List<TakeDamage>();
		parentName_ = transform.parent.name;
	}
	
	
	void FixedUpdate(){
		if(dmgTokens.Count == 0) return;
		// Process individual cooldowns
		foreach (var item in dmgTokens)
		{
			if(item.cooldownRemaining > 0){
				dealtDamage = true;
				item.ProcessCooldown();
			} 
		}
		
		// Debug.Log("Done");
		if(dealtDamage && destroyOnDamage){
			var ctrl = transform.parent.GetComponent<ParticleController>();
			if(ctrl != null) ctrl.DetachParticles();
			transform.parent.gameObject.SetActive(false);
		}
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
		
		foreach (var item in dmgTokens)
		{
			if(item.Contains(tdamage)) return;
		}
		
		dmgTokens.Add(new DamageToken(tdamage, gracePeriod, damageAmount));
	}
	
	// OnTriggerExit is called when the Collider other has stopped touching the trigger.
	protected void OnTriggerExit2D(Collider2D collider)
	{
		TakeDamage tdamage;
		if(!collider || !collider.transform.parent.TryGetComponent<TakeDamage>(out tdamage)) return;
		if (tdamage.dead) return;
		DamageToken? toRemove = null;
		foreach(var token in dmgTokens){
			if(token.Contains(tdamage)) toRemove = token;
		}
		if(toRemove != null){
			dmgTokens.Remove(toRemove);
		}
	}
}


