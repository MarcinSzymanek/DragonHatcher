using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	PlayerInput input_;
	GameObject controlled_;
	Transform playerTf_;
	Spellcaster caster_;
	// Attack attackScript_;
	
	InputAction actionCast;
	InputAction actionGet;
	InputAction actionMove;
	
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
	    
	    itemPicker_ = controlled_.GetComponent<ItemPicker>();
	    caster_ = controlled_.GetComponent<Spellcaster>();
    }

	
	void Start()
	{
		actionGet.performed += OnGet;
		actionCast.performed += OnCast;
    }
    
	// Continuously read input from move action and change player movement based on that
	void Update(){
		if(!enabled_) return;
		var moveDirection = actionMove.ReadValue<Vector2>();
		//Debug.Log(moveDirection);
		moveScript_.ChangeDirection(moveDirection.x, moveDirection.y);
	}
	
	void OnGet(InputAction.CallbackContext context){
		//itemPicker_.OnPickup();
	}
	
	
	void OnCast(InputAction.CallbackContext context){
		if(!enabled_) return;
		caster_.CastSpell(new VectorTarget(playerTf_.position, Math2d.CalcDirection(playerTf_.position, Camera.main.ScreenToWorldPoint(Input.mousePosition))));	
	}
	
	public void DisableGameplayInput(){
		enabled_ = false;
	}
	
	public void EnableGameplayInput(){
		enabled_ = true;
	}
}
