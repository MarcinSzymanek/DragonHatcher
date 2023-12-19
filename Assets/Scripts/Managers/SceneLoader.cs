using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneLoader : MonoBehaviour
{
	GameObject player_;
	MusicController musicController_;
	FadeEffect fade_;
	string loadingScene = "Loading";
	string nextScene = "";
	InputManager input_;
	bool fadeOutDone = false;
	
	static int difficulty = 0;
	
	static SceneLoader instance_;
	
	[field: SerializeField]
	List<GameObject> rewardsList_;

	void Awake(){
		if(instance_ == null){
			instance_ = this;
			SceneProperties.SceneType type = GameObject.FindObjectOfType<SceneProperties>().sceneType;
			if(type == SceneProperties.SceneType.DUNGEON_CRAWL){
				GameObject reward = rewardsList_[UnityEngine.Random.Range(0, rewardsList_.Count)];
				rewardsList_.Remove(reward);
				GameObject.FindObjectOfType<DungeonGenerator>().SetDungeonGenerator(0, reward);
			}
			else{
				GameObject.FindObjectOfType<EnemyGenerator>().SetupSpawners();
			}
		}
		else{
			Destroy(this);
			return;
		}
		fade_ = GameObject.Find("BlackScreen").GetComponent<FadeEffect>();
		musicController_ = GameObject.FindObjectOfType<MusicController>();
		player_ = GameObject.Find("Player");
		input_ = GameObject.FindObjectOfType<InputManager>();
	}
	
	
	// We should be able to load the scene here and set it active in OnFadeOutFinished callback, but I don't have time to figure that out now...
	public void ChangeScene(string scenename){
		Destroy(GameObject.FindObjectOfType<SceneProperties>().gameObject);
		musicController_ = GameObject.FindObjectOfType<MusicController>();
		nextScene = scenename;
		fade_ = GameObject.Find("BlackScreen").GetComponent<FadeEffect>();
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
		fade_ = GameObject.Find("BlackScreen").GetComponent<FadeEffect>();
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
		Destroy(GameObject.FindObjectOfType<CinemachineVirtualCamera>());
	
		Destroy(GameObject.FindObjectOfType<DisableUIComponents>().gameObject);
		
		Debug.Log("SET SCENE TO LOADING");
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingScene));
		
		// Wait for the fade in
		fade_ = GameObject.FindObjectOfType<FadeEffect>();
		musicController_.PlayInterlude();

		yield return new WaitForSeconds(fade_.fadeinTime + 0.1f);
		
		
		// Old scene still exists: load in the new one and move the player object there
		StartCoroutine(LoadNextScene(oldScene));

	}
	
	IEnumerator LoadNextScene(Scene oldScene){
		AsyncOperation op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
		op.allowSceneActivation = false;
		bool fadeOutDone = false;
		fade_ = GameObject.FindObjectOfType<FadeEffect>();
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
		fade_ = GameObject.Find("BlackScreen").GetComponent<FadeEffect>();
		Debug.Log("Fade changed to: " + fade_.name);
		if(!op.isDone) op.completed += FinishSceneLoad;
		else FinishSceneLoad(op);
	}
	
	void FinishSceneLoad(AsyncOperation op){
		input_.EnableGameplayInput();
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
		GameObject.FindObjectOfType<SceneProperties>().difficulty = difficulty;
		if(nextScene == "DungeonGenerator") {
			GameObject reward = rewardsList_[UnityEngine.Random.Range(0, rewardsList_.Count)];
			rewardsList_.Remove(reward);
			GameObject.FindObjectOfType<DungeonGenerator>().SetDungeonGenerator(difficulty, reward);
		}
		
		else GameObject.FindObjectOfType<EnemyGenerator>().SetupSpawners(difficulty);
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
		GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
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
