using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This object does not get destroyed between scenes
public class KeepBetweenScenes : MonoBehaviour
{
	private static KeepBetweenScenes instance_;
	
	void Awake()
    {
    	if (instance_ == null){
    		instance_ = this;
    		DontDestroyOnLoad(this);	
    	}
    	else{
    		if(this != instance_) Destroy(this.gameObject);
    	}
    }
    
}
