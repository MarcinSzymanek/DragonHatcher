using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	FadeEffect fade_;
	string nextScene = "Loading";
	void Awake(){
		fade_ =GameObject.Find("BlackScreen").GetComponent<FadeEffect>();
		fade_.fadeOutFinished += OnFadeOutFinished;
	}
	
	public void ChangeScene(string scenename){
		nextScene = scenename;
		fade_.ScreenFadeOut();
	}
	
	void OnFadeOutFinished(){
		SceneManager.LoadScene(nextScene);	
	}
}
