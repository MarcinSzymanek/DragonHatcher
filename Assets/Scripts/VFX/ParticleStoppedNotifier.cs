using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleStoppedNotifier : MonoBehaviour
{
	public ParticleController ParticleController;
	
	private void OnParticleSystemStopped()
	{
		ParticleController.NotifyParticleSystemFinished();
	}

}
