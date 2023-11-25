using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeathController : MonoBehaviour
{
	Animator anim_;
	Transform weapon_;
	Transform[] children_;
	Collider2D hitboxCollider_;
	Collider2D modelCollider_;
	AudioSource deathAudio_;
	
    void Start()
	{
		bool excludeSelected(Transform t){
			return t.gameObject.name != "Model" && t.gameObject.name != "Hitbox" && t != transform;
		}
		TakeDamage dmgScript = GetComponent<TakeDamage>();
		dmgScript.Death += OnDeath;
		dmgScript.Death += GetComponent<Movement>().OnDeath;
		Transform model = transform.Find("Model");
		anim_ = model.GetComponent<Animator>();
		modelCollider_ = model.GetComponent<Collider2D>();
		var temp = GetComponentsInChildren<Transform>();
		children_ = Array.FindAll(temp, excludeSelected);
		hitboxCollider_ = transform.Find("Hitbox").GetComponent<Collider2D>(); 
		IStopOnDeath[] scriptsToStop_ = GetComponents<IStopOnDeath>();
		foreach(IStopOnDeath script in scriptsToStop_){
			dmgScript.Death += script.OnDeath;
		}
		deathAudio_ = hitboxCollider_.GetComponent<AudioSource>();
    }
	
	public void OnDeath(){
		Debug.Log(gameObject.name + " died!");
		deathAudio_.volume += 0.2f;
		deathAudio_.PlayOneShot(deathAudio_.clip);
		hitboxCollider_.enabled = false;
		modelCollider_.GetComponent<Collider2D>().enabled = false;
		modelCollider_.GetComponent<SpriteRenderer>().sortingOrder--;
		foreach(var child in children_){
			child.gameObject.SetActive(false);
		}
		anim_.SetTrigger("Death");
	}
	
}
