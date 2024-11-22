using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth;
    [field: SerializeField]
    public int maxHealth { get; set; }

    void Awake()
    {
        currentHealth = maxHealth;
    }
   void Update()
    {
        if(maxHealth < currentHealth)
        {
            currentHealth = maxHealth;
        }
    }
}