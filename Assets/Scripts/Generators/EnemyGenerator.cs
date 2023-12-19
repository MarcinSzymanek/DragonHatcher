using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIStrategies;
using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
	// How many enemies should be spawned total
	public int enemiesToSpawn;
	// How fast the enemies should spawn
	public float spawnRate = 1.0f;
	
	// Base delay between enemies spawning
	public float spawnDelay = 2.0f;
	
	// How many enemies are yet to be spawned
	private int enemiesLeft_;
	private int enemiesAlive_;
	
	private List<int> regulateThresholdList;
	
	// Whether or not the generation is on or off
	public bool enabled{get; private set;}
	
	// The scripts responsible for placing enemies in the scene
	private IEnemySpawner[] spawners_;
    
	// Regulate spawn rate based o number of enemies left
	public float regulateIntensity = 1.3f;
	private int regulationCount = 0;
	private int nextThreshold = 0;
	private int difficulty_;
	
	GameController gameController_;
	
	SceneProperties.SceneType sceneType;
	
	void Regulate()
	{
		spawnRate *= regulateIntensity;
		regulationCount++;
		if(regulationCount >= regulateThresholdList.Count){
			nextThreshold = 0;
			return;
		}
		nextThreshold = regulateThresholdList[regulationCount];
	}
	
	// Set AI strategy according to scene type
	public void SetupSpawners(int difficulty = 0){
		difficulty_ = difficulty;
		enemiesToSpawn = enemiesToSpawn + difficulty * enemiesToSpawn;
		StartCoroutine(setupSpawnersAsync());
	}
	
	IEnumerator setupSpawnersAsync(){
		
		while(SceneManager.loadedSceneCount > 1){
			yield return null;
		}
		gameController_ = FindObjectOfType<GameController>();
		
		IAI_Strategy strategy;
		SceneProperties sceneProps = GameObject.FindObjectOfType<SceneProperties>();
		sceneType = sceneProps.sceneType;
		if(sceneProps.sceneType == SceneProperties.SceneType.WAVE_DEFENCE) strategy = new AIStrategies.StrategyTargetEgg();
		else strategy = new AIStrategies.StrategyScanForPlayer();
		enemiesLeft_ = enemiesToSpawn;
		spawners_ = GetComponentsInChildren<IEnemySpawner>();
		foreach(var s in spawners_){
			s.SetDifficulty(difficulty_);
			s.SetAIStrategy(strategy);
		}
		
		regulateThresholdList = new List<int>();
		
		// For each difficulty rating, add a regulation threshold where we increase intensity!
		for(int i = sceneProps.difficulty; i > 0; i--){
			regulateThresholdList.Add((int)(enemiesToSpawn - enemiesToSpawn * 1/(i + 1f)));
		}
		if(regulationCount > 0){	
			nextThreshold = regulateThresholdList[regulationCount];
		}
		
		StartSpawners();		
	}
	
	void StartSpawners(){
		spawners_ = GetComponentsInChildren<IEnemySpawner>();
		Invoke("SpawnContinuously", 5f);		
	}

	void SpawnContinuously(){
		int spawner_index = UnityEngine.Random.Range(0, spawners_.Length);
		var enemy = spawners_[spawner_index].Spawn();
		enemy.GetComponent<DeathController>().objectDied += OnEnemyDeath;
		enemiesLeft_--;
		enemiesAlive_++;
		
		if(enemiesLeft_ < 1){
			return;
		}
		
		if(enemiesLeft_ < nextThreshold){
			Regulate();
		}
		
		Invoke("SpawnContinuously", 1/spawnRate * spawnDelay);
	}
	
	void OnEnemyDeath(object? obj, ObjectDeathArgs args){
		enemiesAlive_--;
		gameController_ = FindObjectOfType<GameController>();
		if(enemiesAlive_ < 1 && enemiesLeft_ < 1){
			Debug.LogWarning("All enemies have been defeated!");
			gameController_.OnWinCondition();
		}
	}
	
    
    
    
}
