using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAnimEvents : MonoBehaviour
{
	
	ManagedSFX sfx_;
	
	void Awake(){
		sfx_ = GetComponentInChildren<ManagedSFX>();	
	}
	
	public void onAnimFinished() {
		if(sfx_ != null) {
			sfx_.transform.parent = transform.root.parent;
			sfx_.OnDetach();
		}
        Destroy(gameObject);
    }
    
	public void playSfx(){
		if(sfx_ != null) sfx_.PlaySfx();	
	}
}
