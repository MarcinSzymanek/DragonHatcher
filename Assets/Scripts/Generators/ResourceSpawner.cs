using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResourceSpawner : Spawner, IResourceSpawner
{
	int maxResCount_ = 10;

	public void SetMaxResource(int count){
		maxResCount_ = count;
	}
	
	void Awake(){
		Spawn(0, new Vector3(1.5f, 1.5f, 0f));
	}
	
	public override GameObject Spawn(int index, Vector3 position){
		var obj = base.Spawn(index, position);
		PickableItem item = obj.GetComponent<PickableItem>();
		item.Count = Random.Range(1, maxResCount_);
		return obj;
	}
	public GameObject Spawn(Vector3 position, Transform parent)
	{
		int index = UnityEngine.Random.Range(0, objectPool.Length);
		var obj = base.Spawn(index, position, parent);
		PickableItem item = obj.GetComponent<PickableItem>();
		item.Count = Random.Range(1, maxResCount_);
		return obj;
    }

    public override GameObject Spawn(int index, Vector3 position, Transform parent){
		var obj = base.Spawn(index, position);
		PickableItem item = obj.GetComponent<PickableItem>();
		item.Count = Random.Range(1, maxResCount_);
		return obj;
	}
}

