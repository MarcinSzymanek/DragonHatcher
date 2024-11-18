using UnityEngine;

/// <summary>
/// This object shall listen to one or more ParticleEndNotifiers and provide a callback
/// </summary>
public interface IParticleSubscriber
{
	void OnParticleEnd();
}
