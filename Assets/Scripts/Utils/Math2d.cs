using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math2d
{	
	// Calculate euclidean distance between object and target positions
	public static float CalcDistance(Vector2 objPos, Vector2 targetPos){
		float x = objPos.x, y = objPos.y;
		float otherx = targetPos.x, othery = targetPos.y;
		
		float distance = Mathf.Sqrt(Mathf.Pow((x - otherx), 2) + Mathf.Pow((y - othery), 2));
		return distance;
	}
	
	// Return a vector with the direction from object position towards target position
	public static Vector2 CalcDirection(Vector2 objectPos, Vector2 targetPos){
		float x = objectPos.x, y = objectPos.y;
		Vector2 dir = new Vector2(targetPos.x - x, targetPos.y - y);
		return dir;
	}
}
