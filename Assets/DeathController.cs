using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeathController : MonoBehaviour
{
	Movement move_;
	AIMeleeSimple ai_;
	Animator anim_;
	Transform model_;
	Transform weapon_;
	
    void Start()
    {
    	// GetComponent<Health>().death += OnDeath;
    	
    	move_ = GetComponent<Movement>();
    	model_ = transform.Find("Model");
    	weapon_ = transform.Find("Weapon");
    	anim_ = model_.GetComponent<Animator>();
    }
	
	public void OnDeath(object? s, EventArgs args){
		Debug.Log(gameObject.name + " died!");
		ai_.OnDeath(s, args);
		move_.ChangeDirection(0, 0);
		model_.GetComponent<Collider2D>().enabled = false;
		model_.GetChild(0).gameObject.SetActive(false);
		model_.GetComponent<SpriteRenderer>().sortingOrder = -9;
		weapon_.gameObject.SetActive(false);
		anim_.SetTrigger("Death");
	}
	
	public void DeathAnimFinished() {
		// Need to clean up the object somehow
	}
}
