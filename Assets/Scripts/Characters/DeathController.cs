using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeathController : MonoBehaviour
{
	Animator anim_;
	Transform model_;
	Transform weapon_;
	Transform[] children_;
	
    void Start()
	{
		TakeDamage dmgScript = GetComponent<TakeDamage>();
		dmgScript.Death += OnDeath;
		dmgScript.Death += GetComponent<Movement>().OnDeath;
    	model_ = transform.Find("Model");
    	anim_ = model_.GetComponent<Animator>();
		children_ = GetComponentsInChildren<Transform>();
		IStopOnDeath[] scriptsToStop_ = GetComponents<IStopOnDeath>();
		foreach(IStopOnDeath script in scriptsToStop_){
			dmgScript.Death += script.OnDeath;
		}
    }
	
	public void OnDeath(){
		Debug.Log(gameObject.name + " died!");
		foreach(var child in children_){
			if(child == transform) continue;
			child.gameObject.SetActive(false);
		}
		anim_.SetTrigger("Death");
	}
	
	public void DeathAnimFinished() {
		// Need to clean up the object somehow
	}
}
