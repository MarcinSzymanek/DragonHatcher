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
	
	void Awake()
	{
		input_ = GameObject.FindObjectOfType<InputManager>();
		sceneLoader_ = FindObjectOfType<SceneLoader>();
		Scene s = SceneManager.GetActiveScene();
		// This is stupid, we should SceneProperties.SceneType instead.
		if(s.name == "TitleScene"){
    		state_ = GameState.title;
		}
		SceneProperties sceneProperties = GameObject.FindObjectOfType<SceneProperties>();
		currentSceneType = sceneProperties.sceneType;
		if(currentSceneType == SceneProperties.SceneType.LOADING) return;
		if(currentSceneType == SceneProperties.SceneType.WAVE_DEFENCE){
			// Set egg dying as the lose condition
			GameObject.Find("DragonEgg").GetComponent<DeathController>().objectDied += GameOver;
		}
		GameObject.Find("Player").GetComponent<DeathController>().objectDied += GameOver;
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
		sceneLoader_.ChangeScene("Loading");
	}
	
	public void OnWinCondition(){
		sceneLoader_.ChangeScene("Loading");	
	}
}
