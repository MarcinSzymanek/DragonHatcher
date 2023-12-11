using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlidingElement : MonoBehaviour
{
	private Vector2 direction;
	public Vector3 offset;
	private RectTransform tf_;
	private bool isSliding = false;
	public float baseSpeed;
	public bool initialDirectionRight;
	
	void Awake()
	{
		tf_	= GetComponent<RectTransform>();
		direction = new Vector2(1, 0);
		if(!initialDirectionRight) direction = new Vector2(-1, 0);
	}
	
	public void Slide(){
		if(isSliding) return;
		isSliding = true;
		Vector3 destination;
		destination = (direction.x > 0)? new Vector3(tf_.localPosition.x + offset.x, tf_.localPosition.y + offset.y) : new Vector3(tf_.localPosition.x - offset.x, tf_.localPosition.y - offset.y);
		
		StartCoroutine(slideRoutine(destination));
	}
	
	// A little silly to use delegates here, but I wanted to learn them
	delegate bool doneDelegate();
	private IEnumerator slideRoutine(Vector3 destination){
		doneDelegate finished;
		if(destination.x > tf_.localPosition.x) finished = () => tf_.localPosition.x > destination.x;
		else finished = () => tf_.localPosition.x < destination.x;
		Debug.Log("Destination: " + destination.ToString());
		while(!finished()){
			tf_.localPosition = new Vector3(tf_.localPosition.x + direction.x * baseSpeed * Time.fixedDeltaTime, tf_.localPosition.y);
			yield return new WaitForFixedUpdate();
		}
		direction = new Vector2(-direction.x, direction.y);
		isSliding = false;
	}
}
