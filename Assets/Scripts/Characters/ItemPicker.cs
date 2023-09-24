using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows a character to pick up objects
public class ItemPicker : MonoBehaviour
{
    Collider2D collider_;
    void Awake()
    {
        collider_ = GetComponent<Collider2D>();
    }

    public void OnPickup(){
        // Get a list of objects in range of being picked and pick the first one
    }

	void OnTriggerEnter2D(Collider2D other){
		if(other.transform.TryGetComponent<PickableItem>(out PickableItem pickable)){
			pickable.TextOn();
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
