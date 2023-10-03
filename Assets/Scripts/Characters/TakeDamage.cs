using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage: MonoBehaviour
{
    // Events
    public event System.Action<int> OnDamageTaken;
    public event System.Action OnDeath;
	
	Health healthComponent;
	
	public void Awake(){
		healthComponent = GetComponent<Health>();
	}
    // TakeDamage function to invoke damage event and possibly destroy object on death
	public void TriggerTakeDamage(int amount)
	{
		Debug.Log("Trig take dmg");
        OnDamageTaken?.Invoke(amount);
        healthComponent.currentHealth -= amount;
        if (healthComponent.currentHealth <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}



