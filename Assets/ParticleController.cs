using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour, IParticleSubscriber
{
	private ParticleSystem[] particleSystems_;
	private int activeParticleSystems_;

	void Awake()
	{
		if(particleSystems_ != null) return;
		particleSystems_ = GetComponentsInChildren<ParticleSystem>();
		var notifiers = GetComponentsInChildren<ParticleStoppedNotifier>();
		foreach(var n in notifiers)
		{
			n.SetSubscriber(this);
		}
	}
	
	private void OnEnable()
	{
		activeParticleSystems_ = particleSystems_.Length;
	}
	
	public void DetachParticles(){
		particleSystems_[0].transform.SetParent(transform.parent);
		particleSystems_[0].Stop();
	}

	public void OnParticleEnd()
	{
		activeParticleSystems_--;
		if(activeParticleSystems_ <= 0)
		{
			gameObject.SetActive(false);
		}
	}

}
