using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleStoppedNotifier : MonoBehaviour
{
	private IParticleSubscriber callbackSubscriber_;

	private void OnParticleSystemStopped()
	{
		callbackSubscriber_.OnParticleEnd();
	}

	public void SetSubscriber(IParticleSubscriber subscriber)
	{
		callbackSubscriber_ = subscriber;
	}

}
