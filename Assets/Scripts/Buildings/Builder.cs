using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
	public Building? SelectedBuilding;
	InputManager input;
	
	void Awake(){
		input = GameObject.FindObjectOfType<InputManager>();
	}
	
	public bool PlaceBuilding(){
		if(SelectedBuilding == null) return false;
		if(SelectedBuilding.TryPlaceBuilding()){
			input.SwitchInputMode(InputManager.InputMode.cast);
			return true;
		}
		return false;
	}
	
	public void CancelBuild(){
		if(SelectedBuilding == null) return;
		GameObject.Destroy(SelectedBuilding.gameObject);
		SelectedBuilding = null;
	}
}
