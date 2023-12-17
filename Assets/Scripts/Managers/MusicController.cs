using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MusicController : MonoBehaviour
{
	AudioSource audios_;
	// Start is called before the first frame update
	
	[field: SerializeField]
	AudioClip musicTrackWave_;
	[field: SerializeField]
	AudioClip musicTrackDungeon_;
	[field: SerializeField]
	AudioClip interludeTrack_;
	
	
	[Range(0f, 1f)]
	public float musicVolume;
	
	public bool playMusic = false;
	
	bool isActive = false;
	
    void Start()
    {
	    audios_ = GetComponent<AudioSource>();
	    audios_.volume = musicVolume;
	    
	    if(playMusic){
	    	PlayMusic();
	    }
	    	
    }
    
	public void PlayInterlude(){
		if(interludeTrack_ == null) return;
		audios_.Stop();
		audios_.PlayOneShot(interludeTrack_);
	}
	
	public void PlayMusic(){
		SceneProperties properties = GameObject.FindObjectOfType<SceneProperties>();
		var sceneType = properties.sceneType;
		AudioClip clip;
		if(sceneType == SceneProperties.SceneType.WAVE_DEFENCE){
			clip = musicTrackWave_;
		}
		else if(sceneType == SceneProperties.SceneType.DUNGEON_CRAWL){
			clip = musicTrackDungeon_;
		}
		else{
			clip = interludeTrack_;
		}
		audios_.PlayOneShot(clip);

	}
	
	public void FadeOutMusic(float duration, System.Action? onFinish = null){
		Debug.LogWarning("FadeOutMusic called");
		StartCoroutine(fadeOut(duration, onFinish));
	}
    
	IEnumerator fadeOut(float duration, System.Action? onFinish){
		// Wait for fadein to finish if needed
		Debug.LogWarning("Musiccontroller fade out");
		while(isActive){
			yield return null;
		}

		// This is necessary, cause otherwise the overlay will block UI buttons and elements
		Debug.Log("Music Fading ouuuuut");
		isActive = true;
		float step = duration/50;
		while(audios_.volume > 0.5){
			audios_.volume -= 0.02f;
			yield return new WaitForSeconds(step);
		}
		while(audios_.volume > 0){
			audios_.volume -= 0.01f;
			yield return new WaitForSeconds(step);
		}
		onFinish?.Invoke();
		isActive = false;
	}
	
	public void StopMusic(){
		audios_.Stop();
	}
	
	public void ResetVolume(){
		audios_.volume = musicVolume;
	}
    
}



