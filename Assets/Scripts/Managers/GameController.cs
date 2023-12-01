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
    
	void Awake()
	{
		Scene s = SceneManager.GetActiveScene();
		if(s.name == "TitleScene"){
    		state_ = GameState.title;
		}
		SceneProperties sceneProperties = GameObject.FindObjectOfType<SceneProperties>();
		if(sceneProperties.sceneType == SceneProperties.SceneType.WAVE_DEFENCE){
			// Set enemy generator AI strategy to target immediatily
		}
		else{
			// Set dungeon generator enemy spawner to trigger via AIScanner
		}
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

}
