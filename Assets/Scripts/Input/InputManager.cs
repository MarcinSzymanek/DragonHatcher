using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	PlayerInput input_;
	GameObject controlled_;
	// Attack attackScript_;
	
	InputAction actionAttack;
	InputAction actionGet;
	InputAction actionMove;
	
	Movement moveScript_;
	ItemPicker itemPicker_;
	
    // Start is called before the first frame update
	void Awake()
    {
	    input_ = GetComponent<PlayerInput>();
	    controlled_ = GameObject.Find("Player");
	    moveScript_ = controlled_.GetComponent<Movement>();
	    // attackScript_ = controlled_.GetComponent<Attack>();
	    InputActionMap controlledMap = input_.actions.FindActionMap("Character");
	    actionAttack = controlledMap.FindAction("Attack");
	    actionGet = controlledMap.FindAction("Get");
		actionMove = controlledMap.FindAction("Move");
	    
	    itemPicker_ = controlled_.GetComponent<ItemPicker>();
    }

	
	void Start()
	{
		actionGet.performed += OnGet;
		actionAttack.performed += OnAttack;
    }
    
	// Continuously read input from move action and change player movement based on that
	void Update(){
		var moveDirection = actionMove.ReadValue<Vector2>();
		//Debug.Log(moveDirection);
		moveScript_.ChangeDirection(moveDirection.x, moveDirection.y);
	}
	
	void OnGet(InputAction.CallbackContext context){
		itemPicker_.OnPickup();
	}
	
	void OnAttack(InputAction.CallbackContext context){
		Debug.Log("Attack button!!!");
		//attackScript_.InitiateAttack();		
	}
}
