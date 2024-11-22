using UnityEngine;
using System.Collections;
using Utils;

public class Dash : MonoBehaviour
{
	Transform tf_;
	// Dash duration
	public float Duration;
	// Dash speed
	public float Speed;
	// Dash force
	public float Force;
	// Whether this action is IsEnabled
	public bool IsEnabled;
	// Whether the player is currently dashing. Only one dash can be IsActive at a time
	public bool IsActive { get; private set; } = false;
	
	private Movement movement_;
	private Rigidbody2D body_;
	public float Cooldown;
	private float cooldown_;
	
	[Header("Trail")]
	// Trail if there is any
	public TrailRenderer DashTrail;
	private GameObject trailObject_;
	private bool useTrail_ = false;
	// Where the echo images from echo effect are stored
	public GameObject EchoImages;
	public Vector2 Offset;

	[Header("Object Pooling")]
	public ObjectPooler DustFXPool;
	
	[Header("Sound effects")]
	public SFXList SFX;
	private AudioClip[] audioClips_;
	private AudioSource audioSource_;

	void Awake()
	{
	    movement_ = GetComponent<Movement>();
	    body_ = GetComponent<Rigidbody2D>();
	    audioSource_ = GetComponentInChildren<AudioSource>();
	    audioClips_ = SFX.Clips;
    	tf_ = transform;
    	
    	if(DashTrail != null)
    	{
    		useTrail_ = true;
    		trailObject_ = DashTrail.gameObject;
    	}
    }
    
	public void TriggerDash()
	{
		if(IsActive || !IsEnabled) return;
		IsActive = true;
		Vector2 dir = movement_.GetDirection();
		push(dir.x, dir.y, Force);
		
		Invoke("reset", Cooldown);
	}
	
	private void reset()
	{
		IsActive = false;
	}
	
	// Push this object in any direction with force amount of force
	private void push(float dirx, float diry, float force)
	{
		if(this is null){
			Debug.LogError("How can this object be destroyed. Does it destroy itself on start somehow!???");
			return;
		}
		StartCoroutine(pushRoutine(dirx, diry, force));
	}
	
	public float LastRemaining;

	private IEnumerator pushRoutine(float x, float y, float force){	
		movement_.LockMovementOwned(this);
		float remaining = 30 * Duration;
		LastRemaining = remaining;
		float startSpeed = force * Speed;
		float endSpeed = force * Speed/4;
		float timeSetup = remaining - (remaining/2);
		float speed;
		if(audioClips_ != null)
		{
			var clip = Utils.Collections.GetRandom(audioClips_);
			audioSource_.PlayOneShot(clip);
		}
		if(useTrail_)
		{
			trailObject_.SetActive(true);
		}
		while(remaining > 0f){
			if(remaining > timeSetup) speed = startSpeed;
			else speed = endSpeed;
			body_.MovePosition((body_.position + new Vector2(x, y) * speed * Time.fixedDeltaTime));
			remaining -= 1;  
			yield return new WaitForFixedUpdate();
		}
		movement_.Stop();
		movement_.UnlockMovement(this);
		if(useTrail_)
		{
			trailObject_.SetActive(false);
		}
		GameObject dustEndFx = DustFXPool.GetFreeObject();
		if(dustEndFx)
		{
			dustEndFx.transform.position = tf_.position;
			dustEndFx.SetActive(true);
		}
	}
	
	private void OnDestroy()
	{
		StopAllCoroutines();
	}

}
