﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterHitFeedback : MonoBehaviour
{
	SpriteRenderer spriteRend_;
	// Locker locker_;
	Animator anim_;
	Movement move_;
	float startMaskStrength_ = 0.8f;
	float maskStrength_;
	
	[field: SerializeField]
	public float FlashFalloffSpeed{get; set;}
	
	const string maskName_ = "_MaskStrength";
	Material mat_;
	
	void Awake(){
		// GetComponent<TakeDamage>().HitEvent += OnHit;
		
		// locker_ = GetComponent<Locker>();
		move_ = GetComponent<Movement>();
		anim_ = transform.Find("Model").GetComponent<Animator>();
	}
	
    // Start is called before the first frame update
    void Start()
	{
		spriteRend_ = transform.Find("Model").GetComponent<SpriteRenderer>();
		mat_ = spriteRend_.material;
    }

	public void OnHit(object? sender, EventArgs args){
		ProcessHit();
	}
	
	public void ProcessHit(){
		Debug.Log("Process hit");
		anim_.SetTrigger("Hit");
		spriteRend_.material.SetFloat(maskName_, startMaskStrength_);
		StartCoroutine(ReduceMask());
		// VfxManager.Instance.InvokeHitStop();		
	}
	
	
	private IEnumerator ReduceMask(){
		maskStrength_ = startMaskStrength_;
		while(maskStrength_ > 0){
			maskStrength_ -= 0.01f * FlashFalloffSpeed;
			mat_.SetFloat(maskName_, maskStrength_);
			yield return null;
		}
		mat_.SetFloat(maskName_, 0);
	}
}