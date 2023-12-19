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
	public bool sandbox = false;

	void Awake()
	{
		input_ = GameObject.FindObjectOfType<InputManager>();
		Scene s = SceneManager.GetActiveScene();
		if(!initialized){
			Debug.LogWarning("Subscribe to ACTIVE SCENE CHANGED");
			SceneManager.activeSceneChanged += InitializeLevel;

				StartCoroutine(WaitForPlayer());
		}
	}
	
	IEnumerator WaitForPlayer(){
		while(player_ == null){
			player_ = GameObject.FindGameObjectWithTag("Player");
			Debug.Log("Waiting for player!");
			yield return new WaitForSeconds(0.2f);
		}
		player_.AddComponent<Unique>();
		Debug.LogWarning("GAMECONTROLLER SUB TO PLAYER DEATH");
		player_.GetComponent<DeathController>().objectDied += GameOver;
	}
	
	void InitializeLevel(Scene next){
		
		currentSceneType = GameObject.FindObjectOfType<SceneProperties>().sceneType;
		if(currentSceneType == SceneProperties.SceneType.LOADING) return;
		if(currentSceneType == SceneProperties.SceneType.WAVE_DEFENCE){
			// Set egg dying as the lose condition
			GameObject.Find("DragonEgg").GetComponent<DeathController>().objectDied += GameOver;
			nextScene_ = "DungeonGenerator";
		}
		if(currentSceneType == SceneProperties.SceneType.START_MENU) Destroy(this);
		else{
			nextScene_ = "WaveDefense";
		}

		var vmcam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
		if(vmcam != null){
			vmcam.Follow = player_.transform;
		}
	}
	
	IEnumerator WaitForScene(Scene next){
		while(SceneManager.GetActiveScene() != next){
			yield return new WaitForSeconds(0.1f);
		}
		InitializeLevel(next);
	}

	void InitializeLevel(Scene current, Scene next){
		if(this == null){
			Destroy(this);
		}
		Debug.Log("Waiting for: current: " + current.name + " next: " + next.name);
		StartCoroutine(WaitForScene(next));
	}

	public void InitializePlayer(GameObject player){
		if(player_ == null){
			player_ = player;
			player.GetComponent<DeathController>().objectDied += GameOver;
		}
		else{
			Destroy(player);
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

	private void GameOver(object? sender, ObjectDeathArgs args){
		Debug.Log(args.ObjectName + " died! Game over...");
		DeathController dc = (DeathController) sender;
		dc.objectDied -= GameOver;
		sceneLoader_ = FindObjectOfType<SceneLoader>();
		// Handle player losing the game: Either player died or the egg got destroyed
		input_.DisableGameplayInput();

		// Cleanup objects which are no longer needed
		Destroy(input_);
		Destroy(FindObjectOfType<ResourceManager>());
		// Scene transition
		sceneLoader_.OnDeath();
	}

	public void OnWinCondition(){
		if(sandbox) return;
		sceneLoader_ = FindObjectOfType<SceneLoader>();
		currentSceneType = GameObject.FindObjectOfType<SceneProperties>().sceneType;
		if(currentSceneType == SceneProperties.SceneType.WAVE_DEFENCE){
			nextScene_ = "DungeonGenerator";
		}
		else{
			nextScene_ = "WaveDefense";
		}
		sceneLoader_.ChangeScene(nextScene_);
	}
	
	// This function is called when the behaviour becomes disabled () or inactive.
	protected void OnDisable()
	{
		Debug.LogWarning("GAMECONTROLLER DISABLED");
		try{
			SceneManager.activeSceneChanged -= InitializeLevel;
			player_.GetComponent<DeathController>().objectDied -= GameOver;
		}
		catch (System.Exception e)
		{
			Debug.LogWarning("Could not unsub");
		}
	}

	protected void OnDestroy(){
		Debug.LogWarning("GAME CONTROLLER DESTROYED");
	}
}
