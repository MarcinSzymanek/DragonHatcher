using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SwitchOnVideoEnd : MonoBehaviour
{
	VideoPlayer player_;
    // Start is called before the first frame update
    void Start()
    {
	    player_ = GetComponent<VideoPlayer>();
	    player_.loopPointReached += SwitchToTitle;
    }

	void SwitchToTitle(VideoPlayer source){
		SceneManager.LoadScene("StartMenu");
	}

    // Update is called once per frame
    void Update()
    {
	    if(Input.anyKey){
	    	player_.Stop();
		    SceneManager.LoadScene("StartMenu");	
	    }
    }
}
