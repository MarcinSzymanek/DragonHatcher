using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision){
		var ctrl = transform.parent.GetComponent<ParticleController>();
		if(ctrl != null) ctrl.DetachParticles();
		Destroy(transform.parent.gameObject);
	}
	
	private void OnTriggerStay2D(Collider2D collision)
	{   
		// Say what it should trigger on
		var ctrl = transform.parent.GetComponent<ParticleController>();
		if(ctrl != null) ctrl.DetachParticles();
		Destroy(transform.parent.gameObject);
    }
}
