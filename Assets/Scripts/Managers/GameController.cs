using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	enum GameState{
		title,
		paused,
		game,
	}
	
	GameState state_;
	InputManager input_;
	SceneLoader sceneLoader_;
	public SceneProperties.SceneType currentSceneType{get; private set;}
	string nextScene_;
	GameObject player_;
	static bool subbed = false;
	static bool initialized = false;
	void Awake()
	{
		input_ = GameObject.FindObjectOfType<InputManager>();
		sceneLoader_ = FindObjectOfType<SceneLoader>();
		player_ = GameObject.FindGameObjectWithTag("Player");
		Scene s = SceneManager.GetActiveScene();
		if(!initialized){	
			Debug.LogWarning("Subscribe to ACTIVE SCENE CHANGED");
			SceneManager.activeSceneChanged += InitializeLevel;
			initialized = true;
		}
		// This is stupid, we should SceneProperties.SceneType instead.
		if(s.name == "TitleScene"){
    		state_ = GameState.title;
		}
	}
	
	void InitializeLevel(){
		// Setup lose condition here
		currentSceneType = GameObject.FindObjectOfType<SceneProperties>().sceneType;
		InitializePlayer();
		if(currentSceneType == SceneProperties.SceneType.LOADING) return;
		if(currentSceneType == SceneProperties.SceneType.WAVE_DEFENCE){
			// Set egg dying as the lose condition
			GameObject.Find("DragonEgg").GetComponent<DeathController>().objectDied += GameOver;
			nextScene_ = "DungeonGenerator";
		}
		else{
			nextScene_ = "WaveDefense";
		}

		var dc = player_.GetComponent<DeathController>();
		if(!subbed){
			Debug.LogWarning("SUB TO PLAYER DEATH");
			player_.GetComponent<DeathController>().objectDied += GameOver;	
			subbed = true;	
		} 
	}
	
	void InitializeLevel(Scene current, Scene next){
		Debug.Log("Current: " + current.name + " next: " + next.name);
		InitializeLevel();
	}
	
	void InitializePlayer(){
		// Load player spells and resources here
	}
	    
	public void StartNewGame(){
		Debug.Log("Starting new session...");
	}
    
	public void Quit(){
		Debug.Log("Application exit.");
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	
	private void GameOver(object? sender, ObjectDeathArgs args){
		// Handle player losing the game: Either player died or the egg got destroyed
		Debug.Log(args.ObjectName + " died! Game over...");
		input_.DisableGameplayInput();
		
		// Cleanup objects which are no longer needed
		Destroy(input_);
		Destroy(FindObjectOfType<ResourceManager>());
		// Scene transition
		sceneLoader_.OnDeath();
	}
	
	public void OnWinCondition(){
		sceneLoader_.ChangeScene(nextScene_);	
	}
}
