using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ManagedSFX : MonoBehaviour
{
	[field: SerializeField]
	private AudioClip[] clips_;
	private AudioSource source_;
	private SFXController controller_;
	
	void Awake(){
		controller_ = GameObject.FindFirstObjectByType<SFXController>();
		source_ = GetComponent<AudioSource>();
	}
	
	public void PlaySfx(){
		if(clips_.Length != 0){
			var clip = clips_[UnityEngine.Random.Range(0, clips_.Length)];
			controller_.PlaySound(source_, clip);
		}
	}
	
	public void OnDetach(){
		StartCoroutine(CleanUpSfx());
	}
	
	IEnumerator CleanUpSfx(){
		while(source_.isPlaying){
			Debug.LogWarning("Sfx waiting for cleanup");
			yield return null;
		}
		Destroy(gameObject);
	}
	
	void OnDestroy(){
		controller_.Detach(source_);
	}
}
