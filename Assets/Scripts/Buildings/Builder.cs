using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
	public Building? SelectedBuilding;
	public List<ResourceCost> SelectedCost;
	InputManager input;
	
	void Awake(){
		input = GameObject.FindObjectOfType<InputManager>();
	}
	
	public bool PlaceBuilding(){
		if(SelectedBuilding == null) return false;
		if(SelectedBuilding.TryPlaceBuilding()){
			if(!ResourceManager.instance.ProcessTransaction(SelectedCost)) CancelBuild();
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
	
	public void UpdateSelected(Building b, List<ResourceCost> cost){
		SelectedBuilding = b;
		SelectedCost = cost;
		Debug.Log("new cost: " + cost[0].amount);
	}
}
