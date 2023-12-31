﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IStopOnDeath
{
	Transform tf_;
	Rigidbody2D body_;
	Animator anim_;
	SpriteRenderer sprite_;
	Locker<Movement> locker_;
	Transform hitboxTf_;
	Transform modelTf_;
	
	private bool directionLock_ = false;
	
	public int Facing = 1;
	public bool WeapEquipped = false;
	
	float dirx_;
	float diry_;
	[field: SerializeField]
	public float Speed{get; set;}
	public float HopSpeed = 3f;

	void Awake(){
		tf_ = gameObject.transform;
		body_ = tf_.GetComponent<Rigidbody2D>();
		anim_ = tf_.GetChild(0).GetComponent<Animator>();
		sprite_ = tf_.GetChild(0).GetComponent<SpriteRenderer>();
		hitboxTf_ = tf_.Find("Hitbox");
		modelTf_ = tf_.Find("Model");
	}
	
    void Start()
	{
		locker_ = new Locker<Movement>();
		
    }

    // Update is called once per frame
	void FixedUpdate()
    {
	    if(!locker_.Locked) Move();
    }
	
	public void ChangeDirection(float dirx, float diry){
		if(directionLock_) return;
		dirx_ = dirx;
		diry_ = diry;
		anim_.SetFloat("dirx", dirx_);
		anim_.SetFloat("diry", diry_);
		
	}
	
	public void LockMovementFor(float time){
		directionLock_ = true;
		Invoke("UnlockMovement", time);
	}
	
	public void UnlockMovement(){
		directionLock_ = false;
	}

	// Push this object in its current direction for time amount of time
	public void HopForward(float time){
		if(!locker_.Locked) StartCoroutine(HopRoutine(time));
	}
	
	// Push this object in any direction with force amount of force
	public void Push(float dirx, float diry, float force){
		directionLock_ = true;
		StartCoroutine(PushRoutine(dirx, diry, force));
	}
	
	IEnumerator PushRoutine(float x, float y, float force){	
		locker_.Lock();
		float remaining = 45;
		float startSpeed = force * HopSpeed;
		float endSpeed = force * HopSpeed/4;
		float timeSetup = remaining - (remaining/2);
		float speed;
		Debug.Log("Initiating Hopping dir: " + x + ", " + y);
		while(remaining > 0f){
			if(remaining > timeSetup) speed = startSpeed;
			else speed = endSpeed;
			body_.MovePosition((body_.position + new Vector2(x, y) * speed * Time.fixedDeltaTime));
			remaining -= 1;  
			yield return new WaitForEndOfFrame();
		}
		Stop();
		directionLock_ = false;
		locker_.Unlock();
	}
	
	IEnumerator HopRoutine(float time){
		directionLock_ = true;
		locker_.Lock();
		float x = dirx_;
		float y = diry_;
		if(time < 0) time = Mathf.Abs(time);
		float remaining = time;
		float startSpeed = HopSpeed/2;
		float midSpeed = HopSpeed*2;
		float timeSetup = remaining - (remaining/4);
		float timeEnding = remaining/4;
		float speed;
		Debug.Log("Initiating Hopping dir: " + x + ", " + y);
		while(remaining > 0f){
			if(remaining > timeSetup) speed = startSpeed;
			else if(remaining > timeEnding) speed = midSpeed;
			else speed = startSpeed;
			body_.MovePosition((body_.position + new Vector2(x, y) * speed * Time.fixedDeltaTime));
			remaining -= 1;  
			yield return new WaitForEndOfFrame();
		}
		Stop();
		directionLock_ = false;
		locker_.Unlock();
	}
	
	public void Move(){
		if(dirx_ == 0 && diry_ == 0) {
			anim_.SetBool("IsMoving", false);
			return;
		}
		
		if(dirx_ < 0 && Facing > 0) {
			// Debug.Log("Flip left");
			Flip();
		}
		else if (dirx_ > 0 && Facing < 0) {
			// Debug.Log("Flip right");			
			Flip();
		}
		//var pos = tf_.position;
		anim_.SetBool("IsMoving", true);
		
		body_.velocity = new Vector3(dirx_, diry_, 0) * Speed * Time.fixedDeltaTime;
		
	}
	
	public void Flip(){
		if(Facing > 0){
			hitboxTf_.localEulerAngles = new Vector3(0, 180, 0);
			modelTf_.localEulerAngles = new Vector3(0, 180, 0);
			Facing = -1;
			return;
		}
		
		hitboxTf_.localEulerAngles = new Vector3(0, 0, 0);
		modelTf_.localEulerAngles = new Vector3(0, 0, 0);
		Facing = 1;
	}
	    
	public void Stop(){
		dirx_ = 0;
		diry_ = 0;
		anim_.SetBool("IsMoving", false);
	}
	
	public void OnDeath(){
		Stop();
	}
}
