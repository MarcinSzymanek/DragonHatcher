using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AIRangedSimple : MonoBehaviour
{
	Movement move_;
	AIScan scanner_;
	[field: SerializeField]
	public float AttackDistance{get; set;}
	
	Vector2? moveTarget_ = null;
	Transform? attackTarget_ = null;
	Transform t_;
	
    // Start is called before the first frame update
    void Start()
    {
	    move_ = GetComponent<Movement>();
	    scanner_ = GetComponentInChildren<AIScan>();
    	scanner_.objectEntered += OnPlayerNoticed;
    	t_ = transform;
    }
	
	public void OnDeath(object? s, EventArgs args){
		StopAllCoroutines();
	}
    
	public void OnPlayerNoticed(object? sender, ObjectEnteredArgs args){
		Debug.Log("Noticed the player!!! Distance: " + Math2d.CalcDistance(t_.position, args.T.position));
		scanner_.objectEntered -= OnPlayerNoticed;
		attackTarget_ = args.T;
		moveTarget_ = (Vector2)(((Transform)attackTarget_).position);
	}
	
	public void InitiateAttack(){
		StartCoroutine(RangedAttackRoutine());
	}
	
	private IEnumerator RangedAttackRoutine(){
		move_.Stop();
		
		for(;;){
			yield return null;
		}
	}
}
