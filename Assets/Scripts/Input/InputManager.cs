using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System;

public class InputManager : MonoBehaviour
{
	public enum InputMode{
		ui,
		build,
		cast
	}
	
	private InputMode mode_ = InputMode.cast;
	
	public GameObject PlayerObject;
	PlayerInput input_;
	GameObject controlled_;
	Transform playerTf_;
	Spellcaster caster_;
	Builder builder_;
	UIBuildingShop shop_;
	PauseMenu pause_;
	InputAction actionCast;
	InputAction actionGet;
	InputAction actionMove;
	InputAction actionDash;
	InputAction actionInteract;
	InputAction actionShop;
	InputAction actionCancel;
	
	Movement moveScript_;
	Dash dashScript_;
	InteractActor interactScript_;
	ItemPicker itemPicker_;
	
	private bool enabled_ = true;
	private bool isBuilding_ = false;
	private bool autofire_ = false;
	
	
	private bool initialized = false;
    // Start is called before the first frame update
	void Awake()
	{
		var sceneProps = GameObject.FindObjectOfType<SceneProperties>();
		
		// No Player Input during Loading/Death scenes
		if(sceneProps.sceneType == SceneProperties.SceneType.LOADING || sceneProps.sceneType == SceneProperties.SceneType.START_MENU) {
			if(!this.enabled_) return;
			this.enabled_ = false;
		}
		
		input_ = GetComponent<PlayerInput>();
		
	    InputActionMap controlledMap = input_.actions.FindActionMap("Character");
	    actionCast = controlledMap.FindAction("Attack");
	    actionGet = controlledMap.FindAction("Get");
		actionMove = controlledMap.FindAction("Move");
		actionDash = controlledMap.FindAction("Dash");
		actionInteract = controlledMap.FindAction("Interact");
		pause_ = FindObjectOfType<PauseMenu>();

        actionShop = controlledMap.FindAction("Shop");
	    actionCancel = controlledMap.FindAction("Cancel"); 
	}
	
	public void SetPlayer(GameObject player){
		controlled_ = player;
		moveScript_ = controlled_.GetComponent<Movement>();
		dashScript_ = controlled_.GetComponent<Dash>();
		itemPicker_ = controlled_.GetComponent<ItemPicker>();
		interactScript_ = controlled_.GetComponentInChildren<InteractActor>();
		caster_ = controlled_.GetComponent<Spellcaster>();
		builder_ = controlled_.GetComponent<Builder>();
		
		pause_ = FindObjectOfType<PauseMenu>();
	}
    
	void Start(){

		SetPlayer(PlayerObject);
		actionCast.started -= OnCastStarted;
		actionCast.started += OnCastStarted;	
		actionCast.performed += OnCastPerformed;
		actionCast.canceled += OnCastCanceled;
		actionDash.performed += OnDashButton;
		actionInteract.performed += OnInteractButton;
		
		actionShop.performed += OnShopButton;
		actionCancel.performed += OnCancel;

		initialized = true;
		shop_ = GameObject.FindObjectOfType<UIBuildingShop>();
		if(shop_ == null) return;
	}
	
	void OnDisable()
	{
		actionCast.started -= OnCastStarted;
		actionCast.performed -= OnCastPerformed;
		actionCast.canceled -= OnCastCanceled;
		actionShop.performed -= OnShopButton;
		actionCancel.performed -= OnCancel;
		actionDash.performed -= OnDashButton;
		initialized = false;	
	}
	
	void OnDestroy()
	{
		actionCast.started -= OnCastStarted;
		actionCast.performed -= OnCastPerformed;
		actionCast.canceled -= OnCastCanceled;
		actionShop.performed -= OnShopButton;
		actionCancel.performed -= OnCancel;
		actionDash.performed -= OnDashButton;
		initialized = false;	
	}
	
	public void ShopCreated(UIBuildingShop shop){
		shop_ = shop;
		shop_.onBuildingCreated += EnterBuildMode;
	}
	
	public void PauseCreated(PauseMenu pause){
		pause_ = pause;
		pause_.resumed += EnableGameplayInput;
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
	
	void FixedUpdate()
	{
		if(autofire_)
		{
			caster_.CastSpell(0);
		}
	}
	
	void OnDashButton(InputAction.CallbackContext context){
		dashScript_.TriggerDash();
	}
	
	void OnInteractButton(InputAction.CallbackContext context){
		interactScript_.Interact();
	}
	
	void OnShopButton(InputAction.CallbackContext context){
		if(shop_ == null) {
			shop_ = GameObject.FindObjectOfType<UIBuildingShop>();
			if(shop_ == null){	
				Debug.LogWarning("No shop in this scene!");
				return ;
			}
		}
		
		shop_.SlidePanel();
	}
	
	void OnCancel(InputAction.CallbackContext context){
		if(isBuilding_) {
			builder_.CancelBuild();
			mode_ = InputMode.cast;
			isBuilding_ = false;
			return;
		}
		Debug.Log("Cancel button pressed");
		//pause_ = GameObject.FindObjectOfType<PauseMenu>();
		if(pause_ != null) {
			pause_.PauseLogic();
			if(pause_.GameIsPaused) DisableGameplayInput();	
		}
		else{
			try {
				pause_ = GameObject.FindObjectOfType<PauseMenu>();
				pause_.resumed += EnableGameplayInput;
				pause_.PauseLogic();
				if(pause_.GameIsPaused) DisableGameplayInput();		
			}
			catch (System.Exception e)
			{
				Debug.LogError("Could not find the pause menu!");
			}
		}
		
	}
	
	void OnCastStarted(InputAction.CallbackContext context)
	{
		if(!enabled_) return;
		if(context.control.device is Pointer){
			caster_.CastSpell(0);
		}
	}
	
	void OnCastPerformed(InputAction.CallbackContext context)
	{
		if(!enabled_) return;
		
		if(context.control.name == "1" || context.control.name == "2" || context.control.name == "3" ||context.control.name == "4"){
			caster_.CastSpell(int.Parse(context.control.name));
			return;
		}	
		
		if(isBuilding_){
			controlled_.GetComponent<Builder>().PlaceBuilding();
			isBuilding_ = false;
		}
		
		// Just return cause UI events are handled elsewhere
		if(mode_ == InputMode.ui) return;

		if(context.interaction is HoldInteraction)
		{
			autofire_ = true;
			return;
		}
	}
	
	void OnCastCanceled(InputAction.CallbackContext context)
	{
		autofire_ = false;
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
