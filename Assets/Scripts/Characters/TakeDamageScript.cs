using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageScript : MonoBehaviour
{
    // Events
    public event System.Action<int> OnDamageTaken;
    public event System.Action OnDeath;

    // TakeDamage function to invoke damage event and possibly destroy object on death
    public void TakeDamage(int amount)
    {
        OnDamageTaken?.Invoke(amount);
        Health healthComponent = GetComponent<Health>();
        healthComponent.currentHealth -= amount;
        if (healthComponent != null)
        {
            healthComponent.currentHealth -= amount;
            if (healthComponent.currentHealth <= 0)
            {
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}


