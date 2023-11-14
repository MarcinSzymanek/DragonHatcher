using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterHitFeedback : MonoBehaviour
{
	SpriteRenderer spriteRend_;
	// Locker locker_;
	Animator anim_;
	Movement move_;
	AudioSource audio_;
	float startMaskStrength_ = 0.1f;
	float maskStrength_;
	
	public AudioClip[] clips_;
	
	[field: SerializeField]
	public float FlashFalloffSpeed{get; set;}
	
	const string maskName_ = "_FlashAmount";
	Material mat_;
	
	void Awake(){
		// GetComponent<TakeDamage>().HitEvent += OnHit;
		
		// locker_ = GetComponent<Locker>();
		GetComponent<TakeDamage>().OnDamageTaken += OnHit;
		move_ = GetComponent<Movement>();
		anim_ = transform.Find("Model").GetComponent<Animator>();
		audio_ = transform.Find("Hitbox").GetComponent<AudioSource>();
	}
	
    // Start is called before the first frame update
    void Start()
	{
		spriteRend_ = transform.Find("Model").GetComponent<SpriteRenderer>();
		mat_ = spriteRend_.material;
    }

	public void OnHit(int dmg){
		ProcessHit();
	}
	
	public void ProcessHit(){
		Debug.Log("Process hit");
		anim_.SetTrigger("Hit");
		if(clips_.Length > 0) audio_.PlayOneShot(clips_[UnityEngine.Random.Range(0, clips_.Length)]);
		spriteRend_.material.SetFloat(maskName_, startMaskStrength_);
		StartCoroutine(ReduceMask());
		// VfxManager.Instance.InvokeHitStop();		
	}
	
	
	private IEnumerator ReduceMask(){
		maskStrength_ = startMaskStrength_;
		while(maskStrength_ > 0){
			maskStrength_ -= 0.001f * FlashFalloffSpeed;
			mat_.SetFloat(maskName_, maskStrength_);
			yield return null;
		}
		mat_.SetFloat(maskName_, 0);
	}
}