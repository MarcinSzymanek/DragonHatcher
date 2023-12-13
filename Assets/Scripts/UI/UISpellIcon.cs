using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpellIcon : MonoBehaviour
{
	Image image_;
	public int Id;
	
	CastDelayIndicator delayIndicator_;
	SpellCooldownIndicator cooldownIndicator_;
	
	void Awake(){
		image_ = GetComponent<Image>();
		delayIndicator_ = GetComponentInChildren<CastDelayIndicator>();
		cooldownIndicator_ = GetComponentInChildren<SpellCooldownIndicator>();
	}

	public void ChangeIcon(Sprite sprite){
		image_.sprite = sprite;
	}
	
	public void StartDelayIndicator(float castDelay){
		delayIndicator_.OnCast(castDelay);
	}
	
	public void StartCooldownIndicator(float cooldown){
		cooldownIndicator_.StartCooldownIndicator(cooldown);
	}
}
