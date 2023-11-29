using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner, IEnemySpawner
{
	public Vector3 offset;
	private Vector3 position;
	
	void Start(){
		position = transform.position + offset;
	}
	
	public GameObject Spawn(){
		// Pick enemy at random
		int index = UnityEngine.Random.Range(0, objectPool.Length);
		var newobj = Instantiate(objectPool[index], position, Quaternion.identity);
		return newobj;
	}
	
	public GameObject Spawn(int index){
		var newobj = Instantiate(objectPool[index], position, Quaternion.identity);
		return newobj;
	}

	public override GameObject Spawn(Vector3 position, Transform parent)
	{
		GameObject enemy = base.Spawn(position, parent);
		return enemy;
	}
}
