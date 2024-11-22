using UnityEngine;


// Play partcles on damage event
[RequireComponent(typeof(ContactDamage))]
public class OnDamageEffect : MonoBehaviour
{
	ContactDamage damageScript_;
	ParticleSystem particles_;
	AudioSource audio_;
	
	[Header("Particles")]
	public bool PlayParticles = false;

	[Header("Audio")]
	//[HideInInspector]
	public bool PlayAudio = false;
	//[HideInInspector]	
	public SFXList SFX;
	private AudioClip[] audioClips_;

	void Start()
	{
		if(PlayParticles)
		{
			try {
				particles_ = GetComponentInChildren<ParticleSystem>();
			}
			catch{
				Debug.LogError(this.name + " requires ParticleSystem component in child");   		
				this.enabled = false;
				return;
			}	
		}
 
		if(PlayAudio)
		{
			try {
				audio_ = GetComponent<AudioSource>();
				audioClips_ = SFX.Clips;
			}
			catch{
				Debug.LogError(this.name + " requires AudioSource and SFXList set to play Audio");		
			}
		}
		damageScript_ = GetComponent<ContactDamage>();
		damageScript_.damageEffectEvent += OnDamageEvent;
	}
    
	private void OnDamageEvent(Rigidbody2D _)
	{
		if(PlayParticles)
		{
			particles_.Play();
		}
		if(PlayAudio)
		{
			var clip = Utils.Collections.GetRandom<AudioClip>(audioClips_);
			audio_.PlayOneShot(clip);
		}
	}

}

