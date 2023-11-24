using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
	// How many enemies should be spawned total
	public int enemyCount;
	// How fast the enemies should spawn
	public float spawnRate = 1.0f;
	
	// Base delay between enemies spawning
	public float spawnDelay = 2.0f;
	
	// How many enemies are yet to be spawned
	private int enemiesLeft_;
	
	// Whether or not the generation is on or off
	public bool enabled{get; private set;}
	
	public bool startOnSceneStart = true;
	// The scripts responsible for placing enemies in the scene
	private IEnemySpawner[] spawners_;
    // Start is called before the first frame update
    void Start()
	{
		enemiesLeft_ = enemyCount;
		spawners_ = GetComponentsInChildren<IEnemySpawner>();
		if(startOnSceneStart){
			Invoke("SpawnContinuously", 5f);
		}
    }

	void SpawnContinuously(){
		Debug.Log(spawners_.Length);
		int spawner_index = UnityEngine.Random.Range(0, spawners_.Length);
		spawners_[spawner_index].Spawn();
		enemiesLeft_--;
		
		if(enemiesLeft_ < 1){
			return;
		}
		
		Invoke("SpawnContinuously", 1/spawnRate * spawnDelay);
	}
    
    
    
    
}
