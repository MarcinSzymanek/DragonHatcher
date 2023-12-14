﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System;

public class UIBuildingShop : MonoBehaviour
{
	private int numberOfItems;
	private Builder builderScript;
	public event EventHandler onBuildingCreated;
	private SlidingElement slidingPanel_;

	// Id of the button and the associated building
	private Dictionary<int, BuildingData> buildings;

	public BuildingData initData;
	void Awake()
	{
		slidingPanel_ = GetComponentInChildren<SlidingElement>();
	    builderScript = GameObject.Find("Player").GetComponent<Builder>();
	    if(builderScript == null){
	    	Destroy(gameObject);
	    	return;
	    }
	    buildings = new Dictionary<int, BuildingData>();
	    UIButton button = GetComponentInChildren<UIButton>();
	    var id = button.Id;
	    button.clicked += OnButtonClick;
	    buildings[id] = initData;
	    
    }
	
	public void AddItem(BuildingData building){
		// Add a building item here
	}
	
	// TODO: Check if the player actually has resources first
	public void OnButtonClick(object? sender, UIButton.ButtonClickArgs args){
		var b = buildings[args.Id];
		if(!ResourceManager.instance.CheckPlayerHasResources(b.cost)){
			Debug.Log("Player tried to buy " + b.name + " but did not have enough resources");	
			return;
		}
		GameObject building = Instantiate(b.prefab, GameObject.Find("PlayerBuildings").transform);
		Debug.Log("Updating cost in builder: " + b.cost.Count + b.cost[0].amount);
		builderScript.UpdateSelected(building.GetComponent<Building>(), b.cost);
		onBuildingCreated?.Invoke(this, new EventArgs());
	}
	
	public void SlidePanel(){
		slidingPanel_.Slide();		
	}
}