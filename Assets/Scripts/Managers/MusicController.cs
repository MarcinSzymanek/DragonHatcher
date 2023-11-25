using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MusicController : MonoBehaviour
{
	AudioSource audios_;
	// Start is called before the first frame update

	public bool playMusic = false;
    void Start()
    {
	    audios_ = GetComponent<AudioSource>();
	    if(playMusic){
	    	audios_.PlayOneShot(audios_.clip);
	    }
    }
	
    // Update is called once per frame
    void Update()
    {
        
    }
}


