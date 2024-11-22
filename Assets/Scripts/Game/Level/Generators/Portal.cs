using UnityEngine;

// Control portal animations and sfx
public class Portal : MonoBehaviour
{
	Animator anim_;
	AudioSource audioSource_;
	AudioClip SFXPortalActive;

    void Awake()
    {
	    audioSource_ = GetComponent<AudioSource>();
	    anim_ = GetComponent<Animator>();
    }
    
	private void playActive()
	{
		//audioSource_.clip = SFXPortalActive;
		audioSource_.Play();
	}

}
