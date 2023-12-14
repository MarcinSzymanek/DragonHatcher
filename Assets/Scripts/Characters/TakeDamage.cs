using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage: MonoBehaviour
{
    // Events
    public event System.Action<int> OnDamageTaken;
	public event System.Action Death;
	public bool dead{get; private set;}
	
	Health healthComponent;
	
	public void Awake(){
		healthComponent = GetComponent<Health>();
		dead = false;
	}
    // TakeDamage function to invoke damage event and possibly destroy object on death
	public void TriggerTakeDamage(int amount)
	{
		if(dead) return;
        OnDamageTaken?.Invoke(amount);
        healthComponent.currentHealth -= amount;
        if (healthComponent.currentHealth <= 0)
        {
        	dead = true;
	        Death?.Invoke();
        }
    }
}



