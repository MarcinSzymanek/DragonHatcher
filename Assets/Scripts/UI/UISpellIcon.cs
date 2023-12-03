using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpellIcon : MonoBehaviour
{
	Image image_;
	public int Id;
	
	void Awake(){
		image_ = GetComponent<Image>();	
	}

	public void ChangeIcon(Sprite sprite){
		image_.sprite = sprite;
	}
}
