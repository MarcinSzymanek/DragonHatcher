using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth = 5;

    // Events
    public event System.Action<int> OnDamageTaken;
    public event System.Action OnDeath;
    void Awake()
    {
        health = maxHealth;
    }

    // TakeDamage function to invoke damage event and possibly destroy object on death
    public void TakeDamage(int amount)
    {
        OnDamageTaken?.Invoke(amount);
        Debug.Log("Ouch");
        if (health <= 0) 
        {
            OnDeath?.Invoke();

            Destroy(gameObject);
        }
    }
}