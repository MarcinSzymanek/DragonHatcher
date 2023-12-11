using System.Collections;
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
	
    // Start is called before the first frame update
	void Awake()
    {
	    input_ = GetComponent<PlayerInput>();
	    controlled_ = GameObject.Find("Player");
	    playerTf_ = controlled_.transform;
	    moveScript_ = controlled_.GetComponent<Movement>();
	    // attackScript_ = controlled_.GetComponent<Attack>();
	    InputActionMap controlledMap = input_.actions.FindActionMap("Character");
	    actionCast = controlledMap.FindAction("Attack");
	    actionGet = controlledMap.FindAction("Get");
		actionMove = controlledMap.FindAction("Move");
	    
	    actionShop = controlledMap.FindAction("Shop");
	    actionCancel = controlledMap.FindAction("Cancel");
	    
	    itemPicker_ = controlled_.GetComponent<ItemPicker>();
	    caster_ = controlled_.GetComponent<Spellcaster>();
	    builder_ = controlled_.GetComponent<Builder>();
	    shop_ = GameObject.FindObjectOfType<UIBuildingShop>();
	    
	    if(shop_ != null) {
		    shop_.onBuildingCreated += EnterBuildMode;
	    	actionShop.performed += OnShopButton;
	    	Debug.Log(actionShop);
	    }
    }

	void EnterBuildMode(object? obj, EventArgs args){
		mode_ = InputMode.build;
	}
	
	void Start()
	{
		actionGet.performed += OnGet;
		actionCast.performed += OnCast;
		actionShop.performed += OnShopButton;
		actionCancel.performed += OnCancel;
    }
    
	// Continuously read input from move action and change player movement based on that
	void Update(){
		if(!enabled_) return;
		var moveDirection = actionMove.ReadValue<Vector2>();
		//Debug.Log(moveDirection);
		moveScript_.ChangeDirection(moveDirection.x, moveDirection.y);
	}
	
	void OnShopButton(InputAction.CallbackContext context){
		Debug.Log("Shop button pressed");
		shop_?.SlidePanel();
	}
	
	void OnCancel(InputAction.CallbackContext context){
		Debug.Log("Cancel button pressed");
		if(mode_ ==	InputMode.build){
			builder_.CancelBuild();
			mode_ = InputMode.cast;
		}
	}
	
	void OnGet(InputAction.CallbackContext context){
		//itemPicker_.OnPickup();
	}
	
	
	void OnCast(InputAction.CallbackContext context){
		if(!enabled_) return;
		
		if(context.control.name == "1" || context.control.name == "2" || context.control.name == "3" ||context.control.name == "4"){
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			caster_.CastSpell(int.Parse(context.control.name), mousePos);
			return;
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
			controlled_.GetComponent<Builder>().PlaceBuilding();
			return;
		}
	}
   
	public void SwitchInputMode(InputMode mode){
		mode_ = mode; 
	}
	
	public void DisableGameplayInput(){
		enabled_ = false;
	}
	
	public void EnableGameplayInput(){
		enabled_ = true;
	}

	public void OnPointerEnterShop(){
		mode_ = InputMode.ui;
	}
	public void OnPointerExitShop(){
		if(mode_ != InputMode.build) mode_ = InputMode.cast;
	}
}
