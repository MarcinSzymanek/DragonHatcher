using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Spawner : MonoBehaviour, ISpawner
{
	
	public GameObject[] objectPool;

	public virtual GameObject Spawn(int index, Vector3 position){
		var newobj = Instantiate(objectPool[index], position, Quaternion.identity);
		return newobj;
	}
	
	public virtual GameObject Spawn(int index, Vector3 position, Transform parent){
		var newobj = Instantiate(objectPool[index], position, Quaternion.identity, parent);
		return newobj;
	}

	public virtual GameObject Spawn(Vector3 position, Transform parent)
	{
        var newObj = Instantiate(objectPool[Random.Range(0, objectPool.Length)], position, Quaternion.identity, parent);
		return newObj;
	}
}

#if UNITY_EDITOR
// Spawn the last resource in the list at the center of scene - for testing
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
#endif
