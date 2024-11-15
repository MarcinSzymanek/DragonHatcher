using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIStrategies;

public class EnemySpawner : Spawner, IEnemySpawner
{
	public Vector3 offset;
	private Vector3 position;
	private IAI_Strategy strategy_;
	private int difficulty_ = 0;
	private const int maxDifficulty = 2;
	
	void Start(){
		position = transform.position + offset;
	}
	
	// Difficulty controls which monsters can be spawned by this object
	// 0 = slimes + archers only
	// 1 =  + skeletons
	// 2 =  + knights
	public void SetDifficulty(int diff){
		difficulty_ = diff;
		// We only have 4 enemies, so max difficulty is 2
		if(difficulty_ > maxDifficulty) difficulty_ = maxDifficulty;
	}
	
	// This could really use some refactoring...
	public GameObject Spawn(){
		// Pick enemy at random
		int index = UnityEngine.Random.Range(0, 2 + difficulty_);
		var obj = Spawn(index);
		return obj;
	}
	
	public GameObject Spawn(int index){
		var newobj = Instantiate(objectPool[index], position, Quaternion.identity);
		newobj.GetComponent<IAIBase>().SetStrategy(strategy_);
		return newobj;
	}

	public GameObject Spawn(Vector3 position, Transform parent)
	{
		int index = UnityEngine.Random.Range(0, 2 + difficulty_);
		GameObject enemy = base.Spawn(index, position, parent);
		enemy.GetComponent<IAIBase>().SetStrategy(strategy_);
		return enemy;
	}
	
	// AIStrategy controls whether enemy will scan for player or attack the egg right away
	public void SetAIStrategy(IAI_Strategy strategy){
		strategy_ = strategy;
	}
}
