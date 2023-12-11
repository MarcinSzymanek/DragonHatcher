using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;


public class Rotator : MonoBehaviour
{
	int counter = 0;
	public Transform target;
	private Transform tf_;
	
    void Awake()
    {
	    tf_ = transform;
    }
    
	void FixedUpdate(){
		if(target == null) return;
		RotateToTarget(target);
	}
    
	private int overflow(int val, int target){
		if(val > target) return val - target*2;
		else if(val < target) return val + target*2;
		else return val;
	}
	
	public void Rotate(float degrees){
		tf_.rotation = Quaternion.Euler(new Vector3(0, 0, degrees));
	}
	
	public void RotateToTarget(Transform target){
		float angle = GetAngleToTarget(target);
		tf_.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}
	
	public float GetAngleToTarget(Transform target){
		float x = target.position.x;
		float y = target.position.y;
		var v1 = float2(x, y);
		var v2 = float2(tf_.position.x, tf_.position.y);
		var t = v1 - v2;
		float angle = Mathf.Atan2(t.y, t.x) * Mathf.Rad2Deg;
		return angle;
	}
	
	
}
