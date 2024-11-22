using System.Collections.Generic;
using UnityEngine;
using AIStrategies;

[RequireComponent(typeof(BoxCollider2D))]
public class PortalSpawner : Spawner, IEnemySpawner
{

	public Vector3 offset;
	private IAI_Strategy strategy_;
	private int difficulty_ = 0;
	private int maxMonsterIndex_;
	private const int maxDifficulty = 2;
	
	private UnityEngine.Bounds spawnBounds_;
	
	private void Awake()
	{
		List<UnityEngine.Bounds> bounds = new List<UnityEngine.Bounds>();
		GetComponent<BoxCollider2D>().GetShapeBounds(bounds, false, false);
		spawnBounds_ = bounds[0];
	}
	
	void Start(){
		maxMonsterIndex_ = objectPool.Length; 
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
		int index = UnityEngine.Random.Range(0, maxMonsterIndex_);
		var obj = Spawn(index);
		return obj;
	}
	
	// Spawn object within spawn radius (box collider bounds)
	public GameObject Spawn(int index){
		
		Vector3 position = Vector3.zero;
		position.x = Random.RandomRange(spawnBounds_.min.x, spawnBounds_.max.x);
		position.y = Random.RandomRange(spawnBounds_.min.y, spawnBounds_.max.y);
		
		try
		{
			var newobj = Instantiate(objectPool[index], position, Quaternion.identity);
			newobj.GetComponent<IAIBase>().SetStrategy(strategy_);
			return newobj;
		}
		catch
		{
			Debug.Break();
			return null;
		}
	}

	public GameObject Spawn(Vector3 position, Transform parent)
	{
		int index = UnityEngine.Random.Range(0, maxMonsterIndex_);
		GameObject enemy = base.Spawn(index, position, parent);
		enemy.GetComponent<IAIBase>().SetStrategy(strategy_);
		return enemy;
	}
	
	// AIStrategy controls whether enemy will scan for player or attack the egg right away
	public void SetAIStrategy(IAI_Strategy strategy){
		strategy_ = strategy;
	}

}
