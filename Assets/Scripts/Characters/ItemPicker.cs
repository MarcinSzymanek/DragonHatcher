using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows a character to pick up objects
public class ItemPicker : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D other){
		if(other.transform.TryGetComponent<IPickable>(out IPickable pickable)){

			pickable.OnPickup();
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		if(other.transform.TryGetComponent<PickableItem>(out PickableItem pickable)){
			pickable.TextOff();
		}
	}
}
