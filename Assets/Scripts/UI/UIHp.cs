using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHp : MonoBehaviour
{
	Slider slider_;
	int hp_;
	public GameObject Tracked;
	
	void Awake(){
		slider_ = GetComponentInChildren<Slider>();
		if(name == "UIPlayerHp") Tracked = GameObject.Find("Player");
		if(name == "UIEggHp") Tracked = GameObject.FindGameObjectWithTag("Egg");
		Tracked.GetComponent<TakeDamage>().OnDamageTaken += ReduceHp;
		
	}
	
    void Start()
	{
		var health = Tracked.GetComponent<Health>();
		hp_ = health.currentHealth;
		slider_.maxValue = health.maxHealth;
		slider_.value = hp_;
    }

    
	void ReduceHp(int dmg){
		hp_ = hp_ -= dmg;
		if(hp_ < 0) hp_ = 0;
		slider_.value = hp_;
	}
}
