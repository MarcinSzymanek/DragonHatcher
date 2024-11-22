using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitbox : MonoBehaviour, Hittable
{
	// TakeDamage dmgScript_;
    // Start is called before the first frame update
    void Start()
    {
	    //dmgScript_ = GetComponentInParent<TakeDamage>();
    }

	public void OnHit(int dmg, Transform source){
		Debug.Log("Hitbox: OnHit!");
		// if(!dmgScript_.Immune) dmgScript_.OnHit(dmg, source);
	}
}
