using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AIStrategies;

public enum State
{
    unaware,
    aware
}

public class AIMeleeSimple : MonoBehaviour, IStopOnDeath, IAIBase
{
    Movement move_;
    AIScan scanner_;
    State state_ = State.unaware;

    [field: SerializeField]
    public float LockOnRefresh { get; set; }
    [field: SerializeField]
    public float AttackDistance { get; set; }

    Vector2? moveTarget_ = null;
    Transform? attackTarget_ = null;

    Transform t_;
    IAI_Strategy strategy_;

    public AIScan scanner { get { return scanner_; } }
    public void SetStrategy(IAI_Strategy strategy)
    {
        strategy_ = strategy;
        strategy.Setup(this);
    }

    void Awake()
    {
        t_ = transform;
        scanner_ = GetComponentInChildren<AIScan>();
        if (LockOnRefresh == 0) LockOnRefresh = 0.3f;
        if (AttackDistance == 0) AttackDistance = 0.7f;
        move_ = GetComponent<Movement>();
    }

    void Start()
    {

    }

    public void OnDeath(object? s, EventArgs args)
    {
        StopAllCoroutines();
    }

    public void OnTargetAcquired(object? sender, ObjectEnteredArgs args)
    {
        // Debug.Log("Noticed the player!!! Distance: " + Math2d.CalcDistance(t_.position, args.T.position));
        scanner_.objectEntered -= OnTargetAcquired;
        attackTarget_ = args.T;
        moveTarget_ = (Vector2)(((Transform)attackTarget_).position);
        StartCoroutine(LockOnTarget());
    }

    // Follow the target checking direction every x seconds
    private IEnumerator LockOnTarget()
    {
        Transform attackTarget = (Transform)attackTarget_;
        Vector2 moveTarget, direction;



        for (; ; )
        {
            // if(locker_.Locked) yield return new WaitForSeconds(LockOnRefresh);
            moveTarget = (Vector2)(attackTarget.position);
            if (Math2d.CalcDistance(t_.position, moveTarget) < AttackDistance) direction = new Vector2(0, 0);
            else
            {
                direction = Math2d.CalcDirection(t_.position, moveTarget).normalized;
                MathVisualise.DrawArrow(transform, direction);
            }
            move_.ChangeDirection(direction.x, direction.y);
            yield return new WaitForSeconds(LockOnRefresh);
        }
    }

    private IEnumerator MoveToTarget()
    {
        Vector2 direction = Math2d.CalcDirection(t_.position, moveTarget_.Value).normalized;
        move_.ChangeDirection(direction.x, direction.y);
        float distance = Math2d.CalcDistance(t_.position, moveTarget_.Value);
        float prevDistance = distance;
        while (distance > 0.10)
        {
            if (distance > prevDistance)
            {
                Vector2 newDir = Math2d.CalcDirection(t_.position, moveTarget_.Value);
                newDir = newDir.normalized;
                move_.ChangeDirection(newDir.x, newDir.y);
            }
            //move_.Move();
            prevDistance = distance;
            distance = Math2d.CalcDistance(t_.position, moveTarget_.Value);
            yield return new WaitForFixedUpdate();
        }
        move_.ChangeDirection(0, 0);
    }

    public void OnDeath()
    {
        StopAllCoroutines();
    }
}