using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// This script loads and sets game settings to/from scriptable object
public class GameSettings : MonoBehaviour
{
	public AudioMixer mainAudioMixer;
	public Settings settingsSo;
	private float volume_;
	private bool fullscreen_;
    // Start is called before the first frame update
    void Start()
	{
		fullscreen_ = settingsSo.Fullscreen;
		volume_ = settingsSo.Volume;
		mainAudioMixer.SetFloat("Volume", volume_);
    	DontDestroyOnLoad(this);
    }

	public void SetVolume(float value){
		float newVolume = value * 0.63f;
		if(value < -70) newVolume = value;
		volume_ = newVolume;
		settingsSo.Volume = newVolume;
		mainAudioMixer.SetFloat("Volume", newVolume);
	}
    
	public void SetFullscreen(bool value){
		fullscreen_ = value;
		settingsSo.Fullscreen = value;
	}
	
	public float GetVolume(){
		return volume_ / 0.63f;
	}
}
