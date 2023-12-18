using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using AIStrategies;

public class AIMeleeSimple2 : MonoBehaviour, IStopOnDeath, IAIBase
{
    Movement move_;
    Animator anim_;
    AIScan scanner_;
    public AIScan scanner { get { return scanner_; } }
    AudioSource audio_;
    [field: SerializeField]
    public float AttackDistance { get; set; }
    public IAI_Strategy strategy;

    Vector2? moveTarget_ = null;
    Transform? attackTarget_ = null;
    Transform t_;
    public Transform firePoint_;
    float distance_to_target_;
    float timeSinceLastAttack;

    float range_close;

    bool animFinished_ = false;
    bool attackTrigger_ = false;

    enum State
    {
        wait_player,
        move_to_target,
        attack,
        attack_finished
    };

    // Move in different directions depending on range
    enum Range
    {
        close,
        out_of_range
    }

    void changeState_()
    {
        pickAction_();
    }

    State state_ = State.wait_player;
    Range range_ = Range.out_of_range;

    void Awake()
    {
        t_ = transform;
	    firePoint_ = t_.Find("firePoint");
        scanner_ = GetComponentInChildren<AIScan>();
        move_ = GetComponent<Movement>();
        anim_ = GetComponentInChildren<Animator>();

        audio_ = transform.Find("mainAudio").GetComponent<AudioSource>();

        GetComponentInChildren<EnemyAnimEvents>().attackFinished += OnAttackFinished;
        range_close = AttackDistance;
    }

    void FixedUpdate()
    {
        timeSinceLastAttack += 1 / 50f;
    }

    public void SetStrategy(IAI_Strategy strat)
    {
        strategy = strat;
        strategy.Setup(this);
    }

    public void OnDeath(object? s, EventArgs args)
    {
        StopAllCoroutines();
    }

    public void OnAttackFinished(object? s, EventArgs args)
    {
        animFinished_ = true;
    }

    public void OnTargetAcquired(object? sender, ObjectEnteredArgs args)
    {
        Debug.Log(t_.name + " acquired " + t_.position);
        var dist = Math2d.CalcDistance(t_.position, args.T.position);
        Debug.Log("Noticed the player!!! Distance: " + dist);
        scanner_.objectEntered -= OnTargetAcquired;
        attackTarget_ = args.T;
        moveTarget_ = (Vector2)(((Transform)attackTarget_).position);
        StartCoroutine(CloseIn());
    }

    private void pickAction_()
    {
        switch (state_)
        {
            case State.attack:
                InitiateAttack();
                break;

            case State.attack_finished:
                attackTrigger_ = false;
                distance_to_target_ = Math2d.CalcDistance(t_.position, attackTarget_.position);

                StartCoroutine(CloseIn());
                break;

            default:
                break;
        }
    }

    // Calculate direction to move to target
    private IEnumerator CloseIn()
	{
        distance_to_target_ = Math2d.CalcDistance(t_.position, attackTarget_.position);
        if (distance_to_target_ < AttackDistance && timeSinceLastAttack > 2.0f) 
        {
            InitiateAttack();

        }

        float prevDistance = distance_to_target_;

        var dir = Math2d.CalcDirection(t_.position, attackTarget_.position);
        move_.ChangeDirection(dir.x, dir.y);
		while (distance_to_target_ > AttackDistance || timeSinceLastAttack < 1.0f)
        {
            // If current distance is farther then last distance, recalculate direction
            if (distance_to_target_ > prevDistance)
            {
            	Debug.Log("Recalculating");
                Vector2 newDir = Math2d.CalcDirection(t_.position, attackTarget_.position);
                newDir = newDir.normalized;
                move_.ChangeDirection(newDir.x, newDir.y);
            }
            prevDistance = distance_to_target_;
            distance_to_target_ = Math2d.CalcDistance(t_.position, attackTarget_.position);
            yield return new WaitForFixedUpdate();
        }
        
	    state_ = State.attack;
        pickAction_();
    }

    Vector2 pickDirection_()
    {
        // Pick direction based on range to target
        Vector2 move_dir;

        switch (getRange())
        {
            case Range.out_of_range:
                // Move in the direction of the target
                move_dir = Math2d.CalcDirection(t_.position, attackTarget_.position);

                return move_dir;

            case Range.close:
                // Close enough, initiate attack
                move_dir = Vector2.zero;
                InitiateAttack();
                return move_dir;
        }

        // The code cannot ever get here...
        Debug.LogError("This should be impossible.");
        return new Vector2();
    }

    // Get range state based on distance to target and thresholds
    private Range getRange()
    {
        if (distance_to_target_ > AttackDistance)
        {
            range_ = Range.out_of_range;
            return range_;
        }
        range_ = Range.close;
        return Range.close;
    }

    public void InitiateAttack()
    {
        state_ = State.attack;
        move_.Stop();
        attackTrigger_ = false;
        animFinished_ = false;
        anim_.Play("Attack");
        StartCoroutine(MeleeAttackRoutine(() => {
            Debug.Log("Attack callback");
        }));
    }

    public void setAttackTrigger()
    {
        attackTrigger_ = true;
    }

    // Follow the target direction, release when animation event happens
    private IEnumerator MeleeAttackRoutine(System.Action callback)
    {
        Vector2 dir = new Vector2(0, 0);
        float angle = 0;
        dir = Math2d.CalcDirection(t_.position, attackTarget_.position);
        angle = Math2d.GetDegreeFromVector(dir, 90);
        firePoint_.localPosition = dir;

        anim_.Play("Attack");
        audio_.PlayOneShot(audio_.clip);
        Spawn_Projectile sp = GetComponent<Spawn_Projectile>();
        sp.Shoot(new VectorTarget(t_.position, dir));

	    while (!animFinished_)
        {
            yield return new WaitForFixedUpdate();
        }

        timeSinceLastAttack = 0;

        state_ = State.attack_finished;
        pickAction_();
    }


    public void OnDeath()
    {
        StopAllCoroutines();
    }
}
