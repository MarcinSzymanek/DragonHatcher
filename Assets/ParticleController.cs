using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
	bool detachOnDisable_ = true;
	ParticleSystem particles_;
    // Start is called before the first frame update
    void Start()
	{
		particles_ = GetComponentInChildren<ParticleSystem>();
		
	}
	
	public void DetachParticles(){
		particles_.transform.SetParent(transform.parent);
		particles_.Stop();
	}

}
