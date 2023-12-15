using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectDeathArgs : EventArgs
{
	public ObjectDeathArgs(string n){
		ObjectName = n;
	}
	public string ObjectName;
}

public class DeathController : MonoBehaviour
{
	Animator anim_;
	Transform weapon_;
	Transform[] children_;
	Collider2D hitboxCollider_;
	Collider2D modelCollider_;
	AudioSource deathAudio_;
	TakeDamage dmgScript_;
	public AudioClip deathGrunt;
	
	public event EventHandler<ObjectDeathArgs> objectDied;
	
    void Start()
	{
		bool excludeSelected(Transform t){
			return t.gameObject.name != "Model" && t.gameObject.name != "Hitbox" && t.gameObject.name != "mainAudio" && t != transform;
		}
		dmgScript_ = GetComponent<TakeDamage>();
		dmgScript_.Death += OnDeath;
		
		Transform model = transform.Find("Model");
		anim_ = model.GetComponent<Animator>();
		modelCollider_ = model.GetComponent<Collider2D>();
		var temp = GetComponentsInChildren<Transform>();
		children_ = Array.FindAll(temp, excludeSelected);
		hitboxCollider_ = transform.Find("Hitbox").GetComponent<Collider2D>(); 
		IStopOnDeath[] scriptsToStop_ = GetComponents<IStopOnDeath>();
		foreach(IStopOnDeath script in scriptsToStop_){
			dmgScript_.Death += script.OnDeath;
		}
		deathAudio_ = transform.Find("mainAudio").GetComponent<AudioSource>();
    }
	
	public void OnDeath(){
		Debug.Log(gameObject.name + " died!");
		dmgScript_.Death -= OnDeath;
		
		// If we set a death audio clip, play it loud
		if(deathGrunt){
			deathAudio_.volume += 0.2f;
			deathAudio_.PlayOneShot(deathGrunt);
		}
		
		hitboxCollider_.gameObject.SetActive(false);
		modelCollider_.GetComponent<Collider2D>().enabled = false;
		modelCollider_.GetComponent<SpriteRenderer>().sortingOrder--;
		
		foreach(var child in children_){
			child.gameObject.SetActive(false);
		}
		
		if(anim_) anim_.SetTrigger("Death");
		Debug.LogWarning("Invoke death event");
		objectDied?.Invoke(this, new ObjectDeathArgs(name));
	}
	
}
