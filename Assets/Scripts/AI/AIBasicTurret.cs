using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class AIBasicTurret : MonoBehaviour
{
	private AIScan scanner;
	private List<Transform> targetPool;
	[SerializeField]
	Transform currentTarget;
	[SerializeField]
	private bool isShooting;
	public float attackDelay = 1.0f;
	private Rotator rotator;
	Spawn_Projectile projectiles;
	
	Animator anim;
	
    void Awake()
	{
		targetPool = new List<Transform>();
		scanner = GetComponentInChildren<AIScan>();
		currentTarget = null;
		rotator = GetComponentInChildren<Rotator>();
		scanner.objectEntered += OnObjectEntered;
		scanner.objectExited += OnObjectExited;
		anim = GetComponentInChildren<Animator>();
		var events = GetComponentInChildren<EnemyAnimEvents>();
		events.arrowReleased += OnArrowReleased;
		events.attackFinished += OnAttackFinished;
		projectiles = GetComponent<Spawn_Projectile>();
	}
	
	void FixedUpdate(){
		if(currentTarget == null) return;
		
	}
	
	// If target died before this method is called, cancel
	void Shoot(){
		if(currentTarget == null || isShooting) return;
		isShooting = true;
		anim.SetTrigger("Attack");
	}
	
	void OnArrowReleased(object? sender, System.EventArgs args){
		// Actually shoot the projectile here
		Vector2 direction = new Vector2(0, 0);
		if(currentTarget != null) direction = Math2d.CalcDirection(transform.position, currentTarget.position);
		projectiles.Shoot(new VectorTarget(transform.position, direction));
	}
	
	// Cleanup after shooting and repeat if target is still alive
	void OnAttackFinished(object? sender, System.EventArgs args){
		isShooting = false;
		anim.SetTrigger("AttackFinished");
		if(currentTarget != null) Invoke("Shoot", attackDelay);
	}
	
	// Add or remove targets to the target pool if they enter/exit scan range
	void OnObjectEntered(object? sender, ObjectEnteredArgs args){
		targetPool.Add(args.T);
		if(currentTarget == null){
			pickNewTarget();
			Shoot();
		}
	}

	void OnObjectExited(object? sender, ObjectEnteredArgs args){
		targetPool.Remove(args.T);
		if(currentTarget == args.T)	pickNewTarget();
	}

	void pickNewTarget(){
		if(targetPool.Count == 0){
			currentTarget = null;
			rotator.target = null;
			return;
		}
		// Pick a random target from the target pool
		currentTarget = targetPool[UnityEngine.Random.Range(0, targetPool.Count)];
		rotator.target = currentTarget;
	}
}
