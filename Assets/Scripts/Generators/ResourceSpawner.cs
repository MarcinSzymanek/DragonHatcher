using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour, ISpawner
{
	public GameObject obj;
    
	public int countMax = 10;

	private int totalResource = 0;

	void Awake(){
		totalResource = 0;
	}

	public GameObject Spawn(Vector3 position){
		var newobj = Instantiate(obj, position, Quaternion.identity);
		PickableItem item = newobj.GetComponent<PickableItem>();
		item.Count = Random.RandomRange(0, countMax);
		totalResource += item.Count;
		return newobj;
	}
	
	public GameObject Spawn(Vector3 position, Transform parent){
		var newobj = Instantiate(obj, position, Quaternion.identity, parent);
		PickableItem item = newobj.GetComponent<PickableItem>();
		item.Count = Random.RandomRange(0, countMax);
		totalResource += item.Count;
		return newobj;
	}
}
