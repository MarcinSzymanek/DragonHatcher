using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
	void Awake(){
		KeepBetweenScenes toClean = GameObject.FindObjectOfType<KeepBetweenScenes>();
		if(toClean != null) {
			Debug.Log("Cleaning up...");
			var objects = toClean.GetComponentsInChildren<DestroyOnStartMenu>();
			foreach (var item in objects)
			{
				Destroy(item.gameObject);
			}
			Destroy(toClean.gameObject);
		}
	}
    public void Play() 
    {
        SceneManager.LoadScene("WaveDefense");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
