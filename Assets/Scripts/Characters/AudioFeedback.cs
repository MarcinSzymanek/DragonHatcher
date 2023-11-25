using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFeedback : MonoBehaviour
{
	public bool randomize_pitch;
	public AudioClip footstep1;
	public AudioClip footstep2;
	private AudioSource audios_;
	private int cycle = 0;
	private float[] pitch_list = {1.0f, 0.8f};
    // Start is called before the first frame update
    void Start()
    {
	    audios_ = GetComponent<AudioSource>();
    }

	public void PlayFootstep(){
		if(cycle == 0){
			audios_.PlayOneShot(footstep1);
		}
		else{
			audios_.PlayOneShot(footstep2);
		}
		cycle++;
		if(cycle > 1){
			cycle = 0;
		}
		if(randomize_pitch){
			setNewPitch();
		} 
	}
	
	void setNewPitch(){
		audios_.pitch = pitch_list[Random.Range(0, 1)];
	}

}
