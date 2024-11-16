using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#nullable enable
public class CharacterHitFeedback : MonoBehaviour
{
	SpriteRenderer spriteRend_;
	// Locker locker_;
	Animator? anim_ = null;
	Movement? move_ = null;
	AudioSource audio_;
	float startMaskStrength_ = 0.1f;
	float maskStrength_;
	
	private bool useHitAnim_ = false;
	public AudioClip[] clips_;
	
	[field: SerializeField]
	public float FlashFalloffSpeed{get; set;}
	
	const string maskName_ = "_FlashAmount";
	Material mat_;
	
	void Awake(){
		// locker_ = GetComponent<Locker>();
		GetComponent<TakeDamage>().DamageTakenEvent += OnHit;
		move_ = GetComponent<Movement>();
		if(transform.Find("Model").TryGetComponent<Animator>(out Animator anim))
		{
			anim_ = anim;	
		}
		audio_ = transform.Find("mainAudio").GetComponent<AudioSource>();
		
		if(anim_ is null) return;
			
		int hitStateId = Animator.StringToHash("hit");	
		if (anim_.HasState(0, hitStateId))
		{
			useHitAnim_ = true;
		}
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
		if(useHitAnim_)
		{
			anim_.SetTrigger("hit");
		}
		if(clips_.Length > 0) audio_.PlayOneShot(clips_[UnityEngine.Random.Range(0, clips_.Length)]);
		spriteRend_.material.SetFloat(maskName_, startMaskStrength_);
		StartCoroutine(ReduceMask());
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