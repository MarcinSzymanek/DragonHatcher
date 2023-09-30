using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    void Awake()
    {
        
    }
    
	public event System.Action attack_triggered;
    
	public void TriggerAttack(){
		attack_triggered?.Invoke();
		GetComponentInChildren<Animator>().Play("Attack");
	}

}
