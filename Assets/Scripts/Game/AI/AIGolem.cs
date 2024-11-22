using UnityEngine;
using AIStrategies;
using System;

public class AIGolem : MonoBehaviour, IAIBase
{
	private Transform tf_;
	private Animator anim_;
	private AudioSource audio_;
	private Transform attackMarker_;
	private Transform firePoint_;
	private IAI_Strategy strategy_;
	private LinearAcceleration linearAcceleration_;
	public LinearVelocityData MovementData;
	
	private GameObject[] targets_;
	private Transform attackTarget_;

	public float LaserAttackRange;
	public float LaserAttackCooldown;
	
	// UNUSED, compability with IAIBase
	private AIScan scanner_;
	public AIScan scanner {get{return scanner_;}}
	
	enum State{
		wait_player,
		move_to_target,
		reposition,
		attack,
		attack_finished
	};

	State state_ = State.wait_player;
	
	void Awake(){
		tf_ = transform;
		anim_ = GetComponentInChildren<Animator>();

		audio_ = transform.Find("mainAudio").GetComponent<AudioSource>();
	    
		//GetComponentInChildren<EnemyAnimEvents>().projectileReleased += OnProjectileRelease;
		//GetComponentInChildren<EnemyAnimEvents>().attackFinished += OnAttackFinished;
    	
		attackMarker_ = tf_.Find("AttackMarker");
		firePoint_ = tf_.Find("firePoint");
    	
	}
	// Start is called before the first frame update
	void Start()
	{
		targets_ = EnemyTracker.Instance.GetTargetsInScene();
	}
	
	public void SetStrategy(IAI_Strategy strat){
		strategy_ = strat;
		strategy_.Setup(this);
	}
	
	public void OnDeath(object? s, EventArgs args){
		StopAllCoroutines();
	}
	
	private float distanceToTarget()
	{
		return Math2d.CalcDistance(tf_.position, attackTarget_.position);
	}
	
	private Vector2 directionToTarget()
	{
		return Math2d.CalcDirection(tf_.position, attackTarget_.position);
	}
	
	public void OnTargetAcquired(object? sender, ObjectEnteredArgs args)
	{
		
	}

}
