using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpellIcon : MonoBehaviour
{
	Image image_;
	public int Id;
	
	CastDelayIndicator delayIndicator;
	void Awake(){
		image_ = GetComponent<Image>();
		delayIndicator = GetComponentInChildren<CastDelayIndicator>();
	}

	public void ChangeIcon(Sprite sprite){
		image_.sprite = sprite;
	}
	
	public void StartDelayIndicator(float castDelay){
		delayIndicator.OnCast(castDelay);
	}
}
