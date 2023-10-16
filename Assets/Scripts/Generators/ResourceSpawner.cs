using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Spawner : MonoBehaviour
{
	public GameObject[] objectPool;
    
	public int countMax = 10;

	private int totalResource = 0;

	void Awake(){
		totalResource = 0;
	}

	public GameObject Spawn(int index, Vector3 position){
		var newobj = Instantiate(objectPool[index], position, Quaternion.identity);
		PickableItem item = newobj.GetComponent<PickableItem>();
		item.Count = Random.Range(0, countMax);
		totalResource += item.Count;
		return newobj;
	}
	
	public GameObject Spawn(int index, Vector3 position, Transform parent){
		var newobj = Instantiate(objectPool[index], position, Quaternion.identity, parent);
		PickableItem item = newobj.GetComponent<PickableItem>();
		item.Count = Random.Range(0, countMax);
		totalResource += item.Count;
		return newobj;
	}
}

// Spawn the last resource in the list at the center of scene
[CustomEditor(typeof(Spawner))]
public class CustomButton : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Spawner myScript = (Spawner)target;
		if (GUILayout.Button("Spawn"))
		{
			myScript.Spawn(myScript.objectPool.Length - 1, new Vector3(0, 0, 0));
		}
	}

}
