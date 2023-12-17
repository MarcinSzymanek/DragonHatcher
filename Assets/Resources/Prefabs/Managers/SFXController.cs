using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
	List<AudioSource> managedAudioSources_;
	
    // Start is called before the first frame update
    void Start()
    {
    	managedAudioSources_ = new	List<AudioSource>();    
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
			volume *= 0.5f; 
		}
		Debug.Log("Starting new sfx with volume: " + volume.ToString() + " , delay: " + delay.ToString());
		StartCoroutine(delayedPlay(sourceToPlay, clip, delay, volume));
		
	}
	
	IEnumerator delayedPlay(AudioSource source, AudioClip clip, float delay, float volume){
		yield return new WaitForSeconds(delay);
		source.PlayOneShot(clip, volume);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
