using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHp : MonoBehaviour
{
	Slider slider_;
	int hp_;
    // Start is called before the first frame update
    void Start()
	{
		GameObject player = GameObject.Find("Player");
	    slider_ = GetComponentInChildren<Slider>();   
		var health = player.GetComponent<Health>();
		hp_ = health.currentHealth;
		player.GetComponent<TakeDamage>().OnDamageTaken += ReduceHp;
		slider_.maxValue = health.maxHealth;
		slider_.value = hp_;
    }

    
	void ReduceHp(int dmg){
		hp_ = hp_ -= dmg;
		if(hp_ < 0) hp_ = 0;
		slider_.value = hp_;
	}
}
