using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows a character to pick up objects
public class ItemPicker : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D other){
		if(other.transform.TryGetComponent<PickableItem>(out PickableItem pickable)){
			pickable.TextOn();
			pickable.OnPickup();
		}
		else{
			Debug.Log("Couldn't get coll2d");
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		if(other.transform.TryGetComponent<PickableItem>(out PickableItem pickable)){
			pickable.TextOff();
		}
	}
}
