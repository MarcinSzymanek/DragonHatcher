using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Allows the item to be picked up
*  
*  Turns on the popup help text when the player is in range  
*/ 
public class PickableItem : MonoBehaviour
{
	AddHoverText textScript_;
	[SerializeReference]
	public ResourceObject resource_data;
	
	void Start(){
		textScript_ = GetComponent<AddHoverText>();
	}
    
	public void TextOn() {
		textScript_.SetText(resource_data.Name);
	}
	
	public void TextOff() {
		textScript_.SetText("");
	}
	
	public void OnPickup(Inventory i) {
		// Add item to players inventory. 
		gameObject.SetActive(false);
	}
}
