﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using AIStrategies;

public class AIRangedSimple : MonoBehaviour, IStopOnDeath, IAIBase
{
	Movement move_;
	Animator anim_;
	AIScan scanner_;
	public AIScan scanner {get{return scanner_;}}
	AudioSource audio_;
	Spawn_Projectile projectileSpawner_;
	[field: SerializeField]
	public float AttackDistance{get; set;}
	public IAI_Strategy strategy;
	
	Vector2? moveTarget_ = null;
	Transform? attackTarget_ = null;
	Transform firepoint_;
	Transform t_;
	Transform attackMarker_;
	float distance_to_target_;
	
	// Thresholds for which range the AI is currently in
	float range_far;
	float range_mid;
	float range_close;
	
	bool animFinished_ = false;
	bool attackTrigger_ = false;
	
	enum State{
		wait_player,
		move_to_target,
		reposition,
		attack,
		attack_finished
	};
	
	// Move in different directions depending on range
	enum Range{
		close,
		mid,
		far,
		out_of_range
	}
	
	void changeState_(){
		pickAction_();
	}
	
	State state_ = State.wait_player;
	Range range_ = Range.out_of_range;
	
	void Awake(){
		t_ = transform;
		scanner_ = GetComponentInChildren<AIScan>();
		move_ = GetComponent<Movement>();
		anim_ = GetComponentInChildren<Animator>();

		audio_ = transform.Find("mainAudio").GetComponent<AudioSource>();
		projectileSpawner_ = GetComponent<Spawn_Projectile>();
	    
		GetComponentInChildren<EnemyAnimEvents>().arrowReleased += OnArrowRelease;
		GetComponentInChildren<EnemyAnimEvents>().attackFinished += OnAttackFinished;
    	
		attackMarker_ = t_.Find("AttackMarker");
		firepoint_ = t_.Find("firePoint");
    	
		range_far = AttackDistance * 0.85f;
		range_mid = AttackDistance * 0.65f;
		range_close = AttackDistance * 0.4f;
	}
    // Start is called before the first frame update
    void Start()
    {
	    
    }
    
	public void SetStrategy(IAI_Strategy strat){
		strategy = strat;
		strategy.Setup(this);
	}
	
	public void OnDeath(object? s, EventArgs args){
		StopAllCoroutines();
	}
	
	public void OnArrowRelease(object? s, EventArgs args){
		attackTrigger_ = true;
	}
	
	public void OnAttackFinished(object? s, EventArgs args){
		animFinished_ = true;
	}
    
	public void OnTargetAcquired(object? sender, ObjectEnteredArgs args){
		var dist = Math2d.CalcDistance(t_.position, args.T.position);
		scanner_.objectEntered -= OnTargetAcquired;
		attackTarget_ = args.T;
		moveTarget_ = (Vector2)(((Transform)attackTarget_).position);
		StartCoroutine(CloseIn());
	}
	
	private void pickAction_(){
		switch (state_)
		{
		case State.attack:
			InitiateAttack();
			break;
		
		case State.attack_finished:
			attackTrigger_ = false;
			distance_to_target_ = Math2d.CalcDistance(t_.position, attackTarget_.position);
				
			StartCoroutine(Reposition());
			break;
	
		case State.reposition:
			// Debug.Log("State: reposition");
			distance_to_target_ = Math2d.CalcDistance(t_.position, attackTarget_.position);
			if(getRange() == Range.out_of_range){
				StartCoroutine(Reposition());
				break;
			}
			int dice = UnityEngine.Random.Range(1, 7);
			if(dice > 4){
				StartCoroutine(Reposition());
				break;
			}
			else{
				InitiateAttack();
				break;
			}
			
		default:
			break;
		}
	} 
	
	// Calculate direction to move to target
	private IEnumerator CloseIn(){
		distance_to_target_ = Math2d.CalcDistance(t_.position, attackTarget_.position);
		if(distance_to_target_ < AttackDistance){
			InitiateAttack();
		}
		
		float prevDistance = distance_to_target_;
		// Debug.Log("Archer CloseIn");
	
		var dir = Math2d.CalcDirection(t_.position, attackTarget_.position);
		move_.ChangeDirection(dir.x, dir.y);
		while(distance_to_target_ > AttackDistance){
			// If current distance is farther then last distance, recalculate direction
			if(distance_to_target_ > prevDistance){
				if(attackTarget_ == null) {
					Debug.LogWarning("AI lost its target");
					break;
				}
				Vector2 newDir = Math2d.CalcDirection(t_.position, attackTarget_.position);
				newDir = newDir.normalized;
				move_.ChangeDirection(newDir.x, newDir.y);
			}
			prevDistance = distance_to_target_;
			distance_to_target_ = Math2d.CalcDistance(t_.position, attackTarget_.position);
			
			yield return new WaitForFixedUpdate();
		}
		state_ = State.attack;
		
		// Debug.Log("Closein finished");
		pickAction_();
	}
	
	private IEnumerator Reposition(){
		state_ = State.reposition;
		Vector2 move_dir = pickDirection_();
		// Debug.Log("Move dir: " + move_dir.ToString());
		MathVisualise.DrawArrow(t_, move_dir, Color.green, 5f);
		
		Vector2 target = (Vector2) t_.position + move_dir;	
		var move_distance = Math2d.CalcDistance(t_.position, target);
		move_.ChangeDirection(move_dir.x, move_dir.y);
		float threshold = 0.02f;
		float prev_move_distance;
		
		int framesBeforeTimeout = 120;
		
		while(move_distance > threshold && framesBeforeTimeout > 0){
			framesBeforeTimeout--;
			prev_move_distance = move_distance;
			move_distance = Math2d.CalcDistance(t_.position, target);
			if(move_distance > prev_move_distance){
				// Recalculate move
				move_dir = pickDirection_();
				target = (Vector2) t_.position + move_dir;
				move_.ChangeDirection(move_dir.x, move_dir.y);
			}
			yield return new WaitForFixedUpdate();
		}
		move_.Stop();
		// Debug.Log("Reposition finished");
		pickAction_();
	}
	
	Vector2 pickDirection_(){
		// Pick direction based on range to target
		int max_degree_from_target;
		int min_degree_from_target;
		Vector2 move_dir;
		int dir_degree;
		switch(getRange()) {
		case Range.out_of_range:
			// Move roughly in the direction of the target
			min_degree_from_target = 0;
			max_degree_from_target = 5;
			dir_degree = UnityEngine.Random.Range(min_degree_from_target, max_degree_from_target);
			dir_degree = UnityEngine.Random.Range(-dir_degree, dir_degree);
			move_dir = Math2d.RotVector(Math2d.CalcDirection(t_.position, attackTarget_.position), dir_degree);
			
			move_dir = findUnblockedDirection_(move_dir);
			
			return move_dir;
		case Range.far:
			// Move roughly in the direction of the target
			min_degree_from_target = 0;
			max_degree_from_target = 35;
			dir_degree = UnityEngine.Random.Range(min_degree_from_target, max_degree_from_target);
			dir_degree = UnityEngine.Random.Range(-dir_degree, dir_degree);
			move_dir = Math2d.RotVector(Math2d.CalcDirection(t_.position, attackTarget_.position), dir_degree);
			
			move_dir = findUnblockedDirection_(move_dir);
			
			return move_dir;
		case Range.mid:
			// Move in any direction
			min_degree_from_target = 0;
			max_degree_from_target = 90;
			dir_degree = UnityEngine.Random.Range(min_degree_from_target, max_degree_from_target);
			dir_degree = UnityEngine.Random.Range(-dir_degree, dir_degree);
			move_dir = Math2d.RotVector(-Math2d.CalcDirection(t_.position, attackTarget_.position), dir_degree);
			
			move_dir = findUnblockedDirection_(move_dir);
			
			return move_dir;
		case Range.close:
			// Move away from the target
			int min_degree_away = 0;
			int max_degree_away = 10;
			dir_degree = UnityEngine.Random.Range(min_degree_away, max_degree_away);
			dir_degree = UnityEngine.Random.Range(-dir_degree, dir_degree);
			move_dir = Math2d.RotVector(-Math2d.CalcDirection(t_.position, attackTarget_.position), dir_degree);
			move_dir = findUnblockedDirection_(move_dir);
			return move_dir;
		}
		
		// The code cannot ever get here...
		Debug.LogError("This should be impossible.");
		return new Vector2();
	}
	
	private Vector2 findUnblockedDirection_(Vector2 move_dir){
		bool done = false;
		int no_iter = 0;
		RaycastHit2D hitData;
		LayerMask blocking = LayerMask.GetMask("Obstacles", "Enemy", "Player");
		while(!done){
			no_iter++;
			if(no_iter > 5){
				Debug.LogWarning("Could not find unblocked direction");
				return move_dir;
			}
			MathVisualise.DrawArrow(t_, move_dir, Color.blue);
			Ray2D ray = new Ray2D(t_.position, move_dir);
			hitData = Physics2D.Raycast(t_.position, move_dir, 0.5f, blocking);
			if (hitData && hitData.transform != transform){
				// Path is blocked, try rotate and recalculate
				Debug.Log("Path blocked! By: " + hitData.transform.gameObject.name);
				move_dir = Math2d.RotVector(move_dir, 25);
				Debug.Log("New move_dir: " + move_dir.ToString());
			}
			else{
				done = true;
			}
		}
		return move_dir;
	}
	
	// Get range state based on distance to target and thresholds
	private Range getRange(){
		if(distance_to_target_ > AttackDistance){
			range_	= Range.out_of_range;
			return range_;
		}
		if(distance_to_target_ > range_mid){
			range_ = Range.far;
			return Range.far;
		}
		if(distance_to_target_ > range_close){
			range_ = Range.mid;
			return Range.mid;
		}
		range_ = Range.close;
		return Range.close;
	}
	
	public void InitiateAttack(){
		state_ = State.attack;
		move_.Stop();
		attackMarker_.gameObject.SetActive(true);
		attackTrigger_ = false;
		animFinished_ = false;
		anim_.Play("Attack");
		StartCoroutine(RangedAttackRoutine(() => {
			Debug.Log("Attack callback");
		}));
	}
	
	public void setAttackTrigger(){
		attackTrigger_ = true;
	}
	
	// Follow the target direction, release when animation event happens
	private IEnumerator RangedAttackRoutine(System.Action callback){
		anim_.Play("Attack");
		audio_.PlayOneShot(audio_.clip);
		float wait_time = 0.2f;
		Vector2 dir = new Vector2(0, 0);
		float angle = 0;
		
		// "Charge" and lock in on target
		while(attackTrigger_ == false){
			if(attackTarget_ == null) attackTarget_ = GameObject.FindGameObjectWithTag("Player").transform;
			dir = Math2d.CalcDirection(t_.position, attackTarget_.position);
			angle = Math2d.GetDegreeFromVector(dir, 90);
			attackMarker_.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
			attackMarker_.localPosition = dir;
			firepoint_.localPosition = dir;
			wait_time -= 0.02f;
			yield return new WaitForSeconds(wait_time);
		}
		
		// Launch arrow
		attackMarker_.gameObject.SetActive(false);
		projectileSpawner_.Shoot(new VectorTarget(t_.position, dir));
		
		while(animFinished_ == false){
			yield return new WaitForFixedUpdate();
		}
		
		state_ = State.attack_finished;
		pickAction_();
	}
	
	public void OnDeath(){
		StopAllCoroutines();
		if(attackMarker_.gameObject.activeSelf) attackMarker_.gameObject.SetActive(false);
	}
}
