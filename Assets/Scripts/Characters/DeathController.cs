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
		bool excludeModel(Transform t){
			return t.gameObject.name != "Model";
		}
		TakeDamage dmgScript = GetComponent<TakeDamage>();
		dmgScript.Death += OnDeath;
		dmgScript.Death += GetComponent<Movement>().OnDeath;
    	model_ = transform.Find("Model");
    	anim_ = model_.GetComponent<Animator>();
		var temp = GetComponentsInChildren<Transform>();
		children_ = Array.FindAll(temp, excludeModel);
	
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
		model_.GetComponent<Collider2D>().enabled = false;
		anim_.SetTrigger("Death");
	}
	
	public void DeathAnimFinished() {
		// Need to clean up the object somehow
	}
}
