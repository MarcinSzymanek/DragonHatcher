using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{
	AudioFeedback audio_;
    // Start is called before the first frame update
    void Start()
    {
	    audio_ = GetComponentInChildren<AudioFeedback>();
    }

	public void PlayFootsteps(){
		audio_.PlayFootstep();
	}
}
