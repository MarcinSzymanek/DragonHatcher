using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum State{
	unaware,
	aware
}

public class AIMeleeSimple : MonoBehaviour
{
	Movement move_;
	AIScan scanner_;
	State state_ = State.unaware;
	
	[field: SerializeField]
	public float LockOnRefresh{get; set;}
	[field: SerializeField]
	public float AttackDistance{get; set;}
	
	Vector2? moveTarget_ = null;
	Transform? attackTarget_ = null;
	
	
	Transform t_;
	
    void Start()
	{
		if(LockOnRefresh == 0) LockOnRefresh = 0.3f;
		if(AttackDistance == 0) AttackDistance = 0.7f;
    	move_ = GetComponent<Movement>();
    	scanner_ = GetComponentInChildren<AIScan>();
    	scanner_.objectEntered += OnPlayerNoticed;
    	t_ = transform;
	}
    
	public void OnDeath(object? s, EventArgs args){
		StopAllCoroutines();
	}
    
	public void OnPlayerNoticed(object? sender, ObjectEnteredArgs args){
		Debug.Log("Noticed the player!!! Distance: " + CalcDistance(args.T.position));
		scanner_.objectEntered -= OnPlayerNoticed;
		attackTarget_ = args.T;
		moveTarget_ = (Vector2)(((Transform)attackTarget_).position);
		StartCoroutine(LockOnTarget());
	}
    
	// Follow the target checking direction every x seconds
	private IEnumerator LockOnTarget(){
		Transform attackTarget = (Transform)attackTarget_;
		Vector2 moveTarget, direction;
		
		for(;;){
			// if(locker_.Locked) yield return new WaitForSeconds(LockOnRefresh);
			moveTarget = (Vector2)(attackTarget.position);
			if(CalcDistance(moveTarget) < AttackDistance) direction = new Vector2(0, 0);
			else{
				direction = CalcDirection(moveTarget).normalized;
			}
			move_.ChangeDirection(direction.x, direction.y);
			yield return new WaitForSeconds(LockOnRefresh);
 		}
	}
    
	private IEnumerator MoveToTarget(){
		Vector2 direction = CalcDirection(moveTarget_.Value).normalized;
		move_.ChangeDirection(direction.x, direction.y);
		float distance = CalcDistance(moveTarget_.Value);
		float prevDistance = distance;
		while(distance > 0.10){
			if(distance > prevDistance){
				Vector2 newDir = CalcDirection(moveTarget_.Value);
				newDir = newDir.normalized;
				move_.ChangeDirection(newDir.x, newDir.y);
			}
			//move_.Move();
			prevDistance = distance;
			distance = CalcDistance(moveTarget_.Value);
			yield return new WaitForFixedUpdate();
		}
		move_.ChangeDirection(0, 0);
	}
	
	private float CalcDistance(Vector2 targetPos){
		float x = t_.position.x, y = t_.position.y;
		float otherx = targetPos.x, othery = targetPos.y;
		
		float distance = Mathf.Sqrt(Mathf.Pow((x - otherx), 2) + Mathf.Pow((y - othery), 2));
		return distance;
	}
	
	private Vector2 CalcDirection(Vector2 targetPos){
		float x = t_.position.x, y = t_.position.y;
		Vector2 dir = new Vector2(targetPos.x - x, targetPos.y - y);
		return dir;
	}
}
