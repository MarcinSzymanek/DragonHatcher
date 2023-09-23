using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 5;

    // Events
    public event System.Action<int> OnDamageTaken;
    public event System.Action OnDeath;
    void Awake()
    {
        currentHealth = maxHealth;
    }

    // TakeDamage function to invoke damage event and possibly destroy object on death
    public void TakeDamage(int amount)
    {
        OnDamageTaken?.Invoke(amount);
        currentHealth -= amount;
        if (currentHealth <= 0) 
        {
            OnDeath?.Invoke();

            Destroy(gameObject);
        }
    }
}