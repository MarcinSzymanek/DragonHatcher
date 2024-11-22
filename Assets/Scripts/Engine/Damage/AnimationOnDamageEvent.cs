using UnityEngine;

[RequireComponent(typeof(ContactDamage))]
public class AnimationOnDamageEvent : MonoBehaviour
{
	ContactDamage damageScript_;
	Animator anim_;
	void Start()
    {
    	
    	try {
    		anim_ = GetComponentInParent<Animator>();
    	}
    	catch{
	    	Debug.LogError("AnimationOnDamageEvent requires animator component in parent");   		
	    	this.enabled = false;
	    	return;
    	}
 
	    damageScript_ = GetComponent<ContactDamage>();
	    damageScript_.damageEffectEvent += OnDamageEvent;
    }
    
	private void OnDamageEvent(Rigidbody2D _)
	{
		anim_.SetTrigger("OnDamageDealt");		
	}

}
