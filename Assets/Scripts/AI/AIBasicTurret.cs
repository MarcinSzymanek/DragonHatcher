using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIBasicTurret : MonoBehaviour
{
	private AIScan scanner;
	private List<Transform> targetPool;
	[SerializeField]
	Transform currentTarget;
	[SerializeField]
	private bool isShooting;
	public float attackDelay = 1.5f;
	private Rotator rotator;
	
    void Awake()
	{
		targetPool = new List<Transform>();
		scanner = GetComponentInChildren<AIScan>();
		currentTarget = null;
		rotator = GetComponentInChildren<Rotator>();
		scanner.objectEntered += OnObjectEntered;
		scanner.objectExited += OnObjectExited;
	}
	
	void FixedUpdate(){
		if(currentTarget == null) return;
		
	}
	
	// Add or remove targets to the target pool if they enter/exit scan range
	void OnObjectEntered(object? sender, ObjectEnteredArgs args){
		targetPool.Add(args.T);
		if(currentTarget == null) pickNewTarget();
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
