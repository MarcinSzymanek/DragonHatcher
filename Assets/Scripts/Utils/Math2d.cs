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
		Vector2 dir = new Vector2(targetPos.x - x, targetPos.y - y).normalized;
		return dir;
	}
	
	public static float Rad2Deg(float rad){
		return (rad * 180)/Mathf.PI;
	}
	
	public static float Deg2Rad(float deg){
		return (deg * Mathf.PI)/180;
	}
	
	public static float GetDegreeFromVector(Vector2 vector, float offset = 0){
		return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - offset;
	}
	
	public static Vector2 RotVector(Vector2 vector, float degrees){
		float rad = Deg2Rad(degrees);
		float x = vector.x * Mathf.Cos(rad) - vector.y * Mathf.Sin(rad);
		float y = vector.x * Mathf.Sin(rad) + vector.y * Mathf.Cos(rad);
		return new Vector2(x, y);		
	}
}
