using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Accelerate the rigidbody over a period of time
[RequireComponent(typeof(Rigidbody2D))]
public class LinearAcceleration : MonoBehaviour
{
	private Rigidbody2D body_;

	// Normalized direction vector
	private Vector3 direction_;
	private float startSpeed_;
	private float endSpeed_;
	public float speed_;
	// How long should speed change apply over (linear change)
	private float speedDuration_;
	public float timeLeft_ = 0;
	public float speedStep_;
	private bool started_ = false;
	private bool ended_ = false;
	
	private void Awake()
	{
		body_ = GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate()
	{
		if(ended_ || !started_) return;
		
		timeLeft_--;
		speed_ = speed_ + speedStep_;
		body_.linearVelocity = speed_ * direction_;

		if(timeLeft_ <= 0)
		{
			ended_ = true;
		}
	}

	public void Initialize(
		Vector3 direction,
		float startSpeed, 
		float endSpeed,
		float speedDuration,
		float delay = 0)
	{
		direction_ = direction;
		startSpeed_ = startSpeed;
		endSpeed_ = endSpeed;
		speedDuration_ = speedDuration;
		timeLeft_ = speedDuration * 100;
		speedStep_ = (endSpeed - startSpeed) / timeLeft_;
		speed_ = startSpeed_;
		
		if(delay > 0) 
		{
			StartCoroutine(activateAfterDelay(delay));
			return;
		}
		
		started_ = true;	
	}
	
	private IEnumerator activateAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		started_ = true;
	}
	
	// Directly set objects linear velocity
	public void SetSpeed(float speed)
	{
		body_.linearVelocity = direction_ * speed;
		ended_ = true;
	}
	
}
