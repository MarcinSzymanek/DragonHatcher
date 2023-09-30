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
	public ResourceID res_id;
	public int Count;
	private string name_;
	
	void Start(){
		textScript_ = GetComponent<AddHoverText>();
		name_ = res_id.ToString();
	}
    
	public void TextOn() {
		textScript_.SetText(name_);
	}
	
	public void TextOff() {
		textScript_.SetText("");
	}
	
	public void OnPickup(Inventory i) {
		// Add item to players inventory. 
		ResourceManager.instance.Add(res_id, Count);
		gameObject.SetActive(false);
	}
}
