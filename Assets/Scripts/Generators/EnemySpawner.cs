using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIStrategies;

public class EnemySpawner : Spawner, IEnemySpawner
{
	public Vector3 offset;
	private Vector3 position;
	private IAI_Strategy strategy_;
	
	void Awake(){
		
	}
	
	void Start(){
		position = transform.position + offset;
	}
	
	// This could really use some refactoring...
	public GameObject Spawn(){
		// Pick enemy at random
		int index = UnityEngine.Random.Range(0, objectPool.Length);
		var obj = Spawn(index);
		return obj;
	}
	
	public GameObject Spawn(int index){
		var newobj = Instantiate(objectPool[index], position, Quaternion.identity);
		newobj.GetComponent<IAIBase>().SetStrategy(strategy_);
		return newobj;
	}

	public override GameObject Spawn(Vector3 position, Transform parent)
	{
		GameObject enemy = base.Spawn(position, parent);
		enemy.GetComponent<IAIBase>().SetStrategy(strategy_);
		return enemy;
	}
	
	public void SetAIStrategy(IAI_Strategy strategy){
		strategy_ = strategy;
	}
}
