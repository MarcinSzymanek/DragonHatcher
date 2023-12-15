﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
	public enum InputMode{
		ui,
		build,
		cast
	}
	
	private InputMode mode_ = InputMode.cast;
	
	PlayerInput input_;
	GameObject controlled_;
	Transform playerTf_;
	Spellcaster caster_;
	Builder builder_;
	UIBuildingShop shop_;
	
	InputAction actionCast;
	InputAction actionGet;
	InputAction actionMove;
	
	InputAction actionShop;
	InputAction actionCancel;
	
	Movement moveScript_;
	ItemPicker itemPicker_;
	
	private bool enabled_ = true;
	private bool isBuilding_ = false;
	
    // Start is called before the first frame update
	void Awake()
	{
		var sceneProps = GameObject.FindObjectOfType<SceneProperties>();
		
		// No Player Input during Loading/Death scenes
		if(sceneProps.sceneType == SceneProperties.SceneType.LOADING) {
			Destroy(this);
			return;
		}
		
		input_ = GetComponent<PlayerInput>();
		
		SetPlayer(GameObject.FindGameObjectWithTag("Player"));
	    InputActionMap controlledMap = input_.actions.FindActionMap("Character");
	    actionCast = controlledMap.FindAction("Attack");
	    actionGet = controlledMap.FindAction("Get");
		actionMove = controlledMap.FindAction("Move");
	    
	    actionShop = controlledMap.FindAction("Shop");
	    actionCancel = controlledMap.FindAction("Cancel");   
	}
    
	public void SetPlayer(GameObject player){
		controlled_ = player;
		playerTf_ = player.transform;
		moveScript_ = player.GetComponent<Movement>();
		itemPicker_ = controlled_.GetComponent<ItemPicker>();
		caster_ = controlled_.GetComponent<Spellcaster>();
		builder_ = controlled_.GetComponent<Builder>();

	}
    
	void Start(){
		actionCast.performed += OnCast;	
		actionShop.performed += OnShopButton;
		actionCancel.performed += OnCancel;
		
		shop_ = GameObject.FindObjectOfType<UIBuildingShop>();
		if(shop_ == null) return;
		
		shop_.onBuildingCreated += EnterBuildMode;
	}

	void EnterBuildMode(object? obj, EventArgs args){
		isBuilding_ = true;
	}
    
	// Continuously read input from move action and change player movement based on that
	void Update(){
		if(!enabled_) return;
		var moveDirection = actionMove.ReadValue<Vector2>();
		moveScript_.ChangeDirection(moveDirection.x, moveDirection.y);
	}
	
	void OnShopButton(InputAction.CallbackContext context){
		Debug.Log("Shop button pressed");
		if(shop_ == null) {
			Debug.LogWarning("No shop in this scene!");
			return;
		}
		shop_.SlidePanel();
	}
	
	void OnCancel(InputAction.CallbackContext context){
		Debug.Log("Cancel button pressed");
		if(isBuilding_){
			builder_.CancelBuild();
			mode_ = InputMode.cast;
			isBuilding_ = false;
		}
	}
	
	void OnCast(InputAction.CallbackContext context){
		if(!enabled_) return;
		
		if(context.control.name == "1" || context.control.name == "2" || context.control.name == "3" ||context.control.name == "4"){
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			caster_.CastSpell(int.Parse(context.control.name), mousePos);
			return;
		}	
		
		if(isBuilding_){
			controlled_.GetComponent<Builder>().PlaceBuilding();
			isBuilding_ = false;
		}
		
		switch(mode_){
		case InputMode.cast:
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			caster_.CastSpell(0, mousePos);
			return;
		
		case InputMode.ui:
			// Just return cause UI events are handled elsewhere
			return;
			
		case InputMode.build:
			// controlled_.GetComponent<Builder>().PlaceBuilding();
			return;
		}
	}
   
	public void SwitchInputMode(InputMode mode){
		mode_ = mode; 
	}
	
	public void DisableGameplayInput(){
		moveScript_.Stop();
		enabled_ = false;
	}
	
	public void EnableGameplayInput(){
		enabled_ = true;
	}

	public void OnPointerEnterShop(){
		mode_ = InputMode.ui;
	}
	public void OnPointerExitShop(){
		mode_ = InputMode.cast;
	}
	
	
}
