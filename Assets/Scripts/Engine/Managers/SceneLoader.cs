using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;


public class SceneLoader : MonoBehaviour
{
	GameObject player_;
	MusicController musicController_;
	//CinemachineCamera camera_;
	Light2D globalLight_;
	BlackScreenFade fade_;
	string loadingScene = "Loading";
	string nextScene = "";
	InputManager input_;
	bool fadeOutDone = false;
	
	static int difficulty = 0;
	
	static SceneLoader instance_;
	
	[field: SerializeField]
	List<GameObject> rewardsList_;
	[field: SerializeField]
	GameObject dummyReward_;
	SceneProperties sceneProps_;

	void Awake(){
		if(instance_ == null){
			instance_ = this;
			sceneProps_ = GameObject.FindObjectOfType<SceneProperties>();
			if(sceneProps_.sceneType == SceneProperties.SceneType.DUNGEON_CRAWL){
				GameObject reward = rewardsList_[UnityEngine.Random.Range(0, rewardsList_.Count)];
				rewardsList_.Remove(reward);
				GameObject.FindObjectOfType<DungeonGenerator>().SetDungeonGenerator(0, reward);
			}
			else if (sceneProps_.sceneType ==	SceneProperties.SceneType.WAVE_DEFENCE){
				fade_ = GameObject.Find("BlackScreen").GetComponent<BlackScreenFade>();
			}
		}
		else{
			Destroy(this);
			return;
		}
		musicController_ = GameObject.FindObjectOfType<MusicController>();
		player_ = GameObject.Find("Player");
		input_ = GameObject.FindObjectOfType<InputManager>();
	}
	
	public void StartGame(){
		if (!(sceneProps_.sceneType == SceneProperties.SceneType.START_MENU)){
			Debug.LogError("Cannot start game: Incorrect SceneType", this);
			return;
		}
		// Set scene type
		sceneProps_.sceneType =	SceneProperties.SceneType.WAVE_DEFENCE;
		
		// Enable relevant managers
		// GameObject.Find("BlackScreen").SetActive(true);
		//GameObject.Find("BlackScreen").SetActive(true);
		//GameObject.Find("UIMainWave").SetActive(true);
		musicController_.PlayInterlude();
		//camera_ = GameObject.FindObjectOfType<CinemachineVirtualCamera>(); 
		globalLight_ = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
		UIMainMenu mainMenu = GameObject.FindObjectOfType<UIMainMenu>();
		this.Invoke("StartMonsterGeneration", 5);
		//StartCoroutine(Utils.Enumerators.DoUntil(
		//	() => {
		//		camera_.m_Lens.OrthographicSize += 0.01f;
		//	},
		//	() => camera_.m_Lens.OrthographicSize >= 6f,
		//	0.02f
		//));
		StartCoroutine(Utils.Enumerators.DoUntilAndThen(
			() => {
				globalLight_.intensity += 0.01f;
			},
			() =>  globalLight_.intensity >= 0.95f
			,
			() => {
				globalLight_.intensity = 0.95f;
			}
		));
		// Fades in/out relevant UI groups
		UIManager.Instance.OnGameStart();
		ActivatePlayer();
	}
	
	void ActivatePlayer(){
		player_.transform.Find("Model").gameObject.SetActive(true);
		player_.GetComponent<Movement>().enabled = true;
		input_.EnableGameplayInput();
	}
	
	
	// We should be able to load the scene here and set it active in OnFadeOutFinished callback, but I don't have time to figure that out now...
	public void ChangeScene(string scenename){
		Destroy(GameObject.FindObjectOfType<SceneProperties>().gameObject);
		musicController_ = GameObject.FindObjectOfType<MusicController>();
		nextScene = scenename;
		fade_ = GameObject.Find("BlackScreen").GetComponent<BlackScreenFade>();
		Debug.Log("Fade changed to: " + fade_.name);
		fade_.fadeOutFinished += OnFadeOutFinished;
		musicController_.FadeOutMusic(
			1f, 
			() => {
				musicController_.StopMusic();
				musicController_.ResetVolume();	
			}
		);
		fade_.ScreenFadeOut();
		input_.DisableGameplayInput();
		
	}
	
	public void OnDeath(){
		fade_ = GameObject.Find("BlackScreen").GetComponent<BlackScreenFade>();
		Debug.Log("Fade changed to: " + fade_.name);
		fade_.ScreenFadeOut(OnDeathFadeout);
		musicController_.FadeOutMusic(
			1f, 
			() => {
				musicController_.StopMusic();
				musicController_.ResetVolume();	
			}
		);
		input_.DisableGameplayInput();
	}
	
	void OnDeathFadeout(){
		SceneManager.LoadScene("GameOver");
		Invoke("OnDeathFinished", 2.5f);
	}
	
	void OnDeathFinished(){
		fade_ = GameObject.Find("BlackScreen").GetComponent<BlackScreenFade>();
		fade_.ScreenFadeOut( () =>{
			SceneManager.LoadScene("StartMenu");
		},
			1f);
	}
	
	void OnFadeOutFinished(){
		fade_.fadeOutFinished -= OnFadeOutFinished;
		// If we don't do this, the black screen overlay will not persist
		GameObject.FindObjectOfType<DisableUIComponents>().DisableUI();
		Debug.Log("OnFadeOutFinished");
		StartCoroutine(LoadLoadingScene());
	}
	
	IEnumerator LoadLoadingScene(){
		var oldScene = SceneManager.GetActiveScene();
		AsyncOperation op = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
		
		while(!op.isDone){
			Debug.Log("loading scene not done...");
			
			yield return new WaitForSeconds(0.3f);
		}
		Destroy(GameObject.FindGameObjectWithTag("GlobalLight"));
		Destroy(GameObject.FindObjectOfType<Grid>().gameObject);
		//Destroy(GameObject.FindObjectOfType<CinemachineVirtualCamera>());
	
		Destroy(GameObject.FindObjectOfType<DisableUIComponents>().gameObject);
		
		Debug.Log("SET SCENE TO LOADING");
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingScene));
		
		// Wait for the fade in
		fade_ = GameObject.FindObjectOfType<BlackScreenFade>();
		musicController_.PlayInterlude();

		yield return new WaitForSeconds(fade_.fadeinTime + 0.1f);
		
		
		// Old scene still exists: load in the new one and move the player object there
		StartCoroutine(LoadNextScene(oldScene));

	}
	
	IEnumerator LoadNextScene(Scene oldScene){
		if(difficulty > 3) nextScene = "WinScreen";
		AsyncOperation op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
		op.allowSceneActivation = false;
		bool fadeOutDone = false;
		fade_ = GameObject.FindObjectOfType<BlackScreenFade>();
		fade_.ScreenFadeOut(() => {
			Debug.LogWarning("Finish fadeout");
			if(oldScene.name == "DungeonGenerator"){	
				difficulty++;
			}
			fadeOutDone = true;
		});
		while(!fadeOutDone ){
			yield return new WaitForSeconds(0.1f);
		}
		op.allowSceneActivation = true;
		if(player_ != null){
			input_ = GameObject.FindObjectOfType<InputManager>();
			input_.SetPlayer(player_);
			SceneManager.MoveGameObjectToScene(player_, SceneManager.GetSceneByName(nextScene));
		}
		op = SceneManager.UnloadSceneAsync(oldScene);
		fade_ = GameObject.Find("BlackScreen").GetComponent<BlackScreenFade>();
		Debug.Log("Fade changed to: " + fade_.name);
		if(!op.isDone) op.completed += FinishSceneLoad;
		else FinishSceneLoad(op);
	}
	
	void FinishSceneLoad(AsyncOperation op){
		input_.EnableGameplayInput();
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
		GameObject.FindObjectOfType<SceneProperties>().difficulty = difficulty;
		if(nextScene == "DungeonGenerator") {
			GameObject reward;
			if(rewardsList_.Count != null){
				reward = rewardsList_[UnityEngine.Random.Range(0, rewardsList_.Count)];
				rewardsList_.Remove(reward);
			}
			else{
				reward = dummyReward_;
			}
			GameObject.FindObjectOfType<DungeonGenerator>().SetDungeonGenerator(difficulty, reward);
		}
		
		var operation = SceneManager.UnloadSceneAsync(loadingScene);
		// Check if there are player copies, and destroy them
		var players = GameObject.FindGameObjectsWithTag("Player");
		foreach(var player in players){
			Unique unique = player.GetComponent<Unique>();
			if(unique == null) Destroy(player);
			else{
				player.transform.position = Vector3.zero;
			}
		}
		//GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
		StartCoroutine(QueueUpMusic(operation));
	}
	
	IEnumerator QueueUpMusic(AsyncOperation op){
		while(!op.isDone){
			Debug.Log("Waiting for Loading to unload");
			yield return null;
		}
		Debug.Log(GameObject.FindObjectOfType<SceneProperties>().sceneType.ToString());
		musicController_.PlayMusic();
	}
	
	IEnumerator CheckAsyncOperation(AsyncOperation operation){
		while(!operation.isDone){
			Debug.Log("Operation progress: " + operation.progress.ToString());
			yield return new WaitForSeconds(0.3f);
		}
	}
}
