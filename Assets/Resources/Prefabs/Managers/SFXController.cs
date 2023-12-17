using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
	List<AudioSource> managedAudioSources_;
	
	float timeSinceLastPlayed = 1f;
	
    // Start is called before the first frame update
    void Start()
    {
    	managedAudioSources_ = new	List<AudioSource>();    
    }
    
	void FixedUpdate(){
		timeSinceLastPlayed += 1/50f;
	}
    
	public void Detach(AudioSource source){
		if(managedAudioSources_.Contains(source)) managedAudioSources_.Remove(source);
	}
    
	public void PlaySound(AudioSource sourceToPlay, AudioClip clip){
		if(!managedAudioSources_.Contains(sourceToPlay)) managedAudioSources_.Add(sourceToPlay);
		float delay = 0f;
		float volume = 1f;
		foreach (var source in managedAudioSources_)
		{
			if(source.isPlaying) delay += 0.02f;
		}
		Debug.Log("Starting new sfx with volume: " + volume.ToString() + " , delay: " + delay.ToString());
		StartCoroutine(delayedPlay(sourceToPlay, clip, delay, volume));
		
	}
	
	IEnumerator delayedPlay(AudioSource source, AudioClip clip, float delay, float volume){
		yield return new WaitForSeconds(delay);
		if(timeSinceLastPlayed < 0.10f) volume *= 0.2f; 
		source.PlayOneShot(clip, volume);
		timeSinceLastPlayed = 0f;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
