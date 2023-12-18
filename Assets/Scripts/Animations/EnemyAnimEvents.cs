using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAnimEvents : MonoBehaviour
{
	AudioFeedback audio_;
	public event EventHandler attackFinished;
	public event EventHandler arrowReleased;
    // Start is called before the first frame update
    void Start()
    {
	    audio_ = GetComponentInChildren<AudioFeedback>();
    }

	public void PlayFootsteps(){
		audio_.PlayFootstep();
	}
	
	public void ReleaseArrow(){
		arrowReleased?.Invoke(this, new EventArgs());
	}
	
	public void AttackAnimFinished(){
		Debug.Log("are we being called");
		attackFinished?.Invoke(this, new EventArgs());
	}
	
	public void DeathAnimFinished(){
		GetComponent<Animator>().enabled = false;
	}
}
