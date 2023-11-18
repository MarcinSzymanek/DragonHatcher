using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Encapsulates vector targetting (origin + direction)
public class VectorTarget
{
	public Vector2 origin{get; private set;}
	public Vector2 direction{get; private set;}
	public float angle{get; private set;}
	
	public VectorTarget(Vector2 o, Vector2 dir){
		origin = o;
		direction = dir;
		angle = Math2d.GetDegreeFromVector(direction);
	}
	
	public VectorTarget(Transform origin_tf, Vector2 dir){
		origin = origin_tf.position;
		direction = dir;
		angle = Math2d.GetDegreeFromVector(direction);
	}
}
