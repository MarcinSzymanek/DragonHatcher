using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	InputManager input;
	
	void Awake(){
		input = GameObject.FindObjectOfType<InputManager>();
	}
	
	public void OnPointerEnter(PointerEventData data){
		input.OnPointerEnterShop();
	}
	
	public void OnPointerExit(PointerEventData data){
		input.OnPointerExitShop();
	}
}
