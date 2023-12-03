using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffect : MonoBehaviour
{
	Transform tf_;
	public float startSpeed = 0.04f;
	private float speedThreshold = 0.00001f;
	public float floatingSpeed;
	public bool enabled = true;
	public float dampening = 0.9f;
	
	private float directiony;
	public float threshUp;
	private float threshDown;
	private float dampThresh;
	private float starty;
	
	void Awake(){
		tf_ = transform;
		directiony = 1f;
		dampThresh = tf_.position.y + 0.25f;
		starty = tf_.position.y;
		floatingSpeed = startSpeed;
	}
	
	void FixedUpdate()
    {
	    if(!enabled) return;
	    
	    if(directiony > 0 && tf_.position.y > dampThresh) floatingSpeed = floatingSpeed*dampening; 
	    else if(directiony < 0 && tf_.position.y < dampThresh) floatingSpeed = floatingSpeed*dampening;
	    
	    float speed = directiony * floatingSpeed * Time.fixedDeltaTime;
	    tf_.position = new Vector3(tf_.position.x, tf_.position.y + (directiony * floatingSpeed * Time.fixedDeltaTime));
	    
	    if(floatingSpeed < speedThreshold){
	    	directiony = -directiony;
	    	floatingSpeed = startSpeed;
	    }
    }
}
