using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHp : MonoBehaviour
{
	Slider slider_;
	int hp_;
	public GameObject Tracked;
    // Start is called before the first frame update
    void Start()
	{
	    slider_ = GetComponentInChildren<Slider>();   
		var health = Tracked.GetComponent<Health>();
		hp_ = health.currentHealth;
		Tracked.GetComponent<TakeDamage>().OnDamageTaken += ReduceHp;
		slider_.maxValue = health.maxHealth;
		slider_.value = hp_;
    }

    
	void ReduceHp(int dmg){
		hp_ = hp_ -= dmg;
		if(hp_ < 0) hp_ = 0;
		slider_.value = hp_;
	}
}
