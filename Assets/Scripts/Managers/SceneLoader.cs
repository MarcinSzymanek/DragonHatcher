using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneLoader : MonoBehaviour
{
	GameObject player_;
	FadeEffect fade_;
	string loadingScene = "Loading";
	string nextScene = "";
	InputManager input_;
	bool fadeOutDone = false;
	
	void Awake(){
		fade_ = GameObject.Find("BlackScreen").GetComponent<FadeEffect>();
		player_ = GameObject.Find("Player");
		input_ = GameObject.FindObjectOfType<InputManager>();
	}
	
	
	// We should be able to load the scene here and set it active in OnFadeOutFinished callback, but I don't have time to figure that out now...
	public void ChangeScene(string scenename){
		nextScene = scenename;
		fade_ = GameObject.Find("BlackScreen").GetComponent<FadeEffect>();
		Debug.Log("Fade changed to: " + fade_.name);
		fade_.fadeOutFinished += OnFadeOutFinished;
		fade_.ScreenFadeOut();
		input_.DisableGameplayInput();
		
	}
	
	public void OnDeath(){
		fade_ = GameObject.Find("BlackScreen").GetComponent<FadeEffect>();
		Debug.Log("Fade changed to: " + fade_.name);
		fade_.ScreenFadeOut(OnDeathFadeout);
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
			fadeOutDone = true;
		});
		while(!fadeOutDone ){
			yield return new WaitForSeconds(0.5f);
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
		SceneManager.UnloadSceneAsync(loadingScene);
		// Check if there are player copies, and destroy them
		var players = GameObject.FindGameObjectsWithTag("Player");
		foreach(var player in players){
			Unique unique = player.GetComponent<Unique>();
			if(unique == null) Destroy(player);
		}
		GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	IEnumerator CheckAsyncOperation(AsyncOperation operation){
		while(!operation.isDone){
			Debug.Log("Operation progress: " + operation.progress.ToString());
			yield return new WaitForSeconds(0.3f);
		}
	}
}
