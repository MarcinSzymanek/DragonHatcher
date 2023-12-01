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
    
	void Awake()
	{
		input_ = GameObject.FindObjectOfType<InputManager>();
		
		Scene s = SceneManager.GetActiveScene();
		if(s.name == "TitleScene"){
    		state_ = GameState.title;
		}
		SceneProperties sceneProperties = GameObject.FindObjectOfType<SceneProperties>();
		if(sceneProperties.sceneType == SceneProperties.SceneType.WAVE_DEFENCE){
			// Set enemy generator AI strategy to target immediatily
			// Set egg dying as the lose condition
			GameObject.Find("DragonEgg").GetComponent<DeathController>().objectDied += GameOver;
		}
		else{
			// Set dungeon generator enemy spawner to trigger via AIScanner
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
		
	}

}
