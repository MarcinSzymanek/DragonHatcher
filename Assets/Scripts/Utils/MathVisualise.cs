using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathVisualise : MonoBehaviour
{
	// Draw a line from transform in the direction specified
	public static void DrawLine(Transform t, Vector2 direction, float duration = 0.5f){
		Vector3 start = t.position;
		Vector3 dir = direction;
		Vector3 tip = start + dir;
		Debug.DrawLine(t.position, tip, Color.red, duration);
	}
	
	// Draw a line from transform in the direction specified
	public static void DrawLine(Transform t, Vector2 direction, Color color, float duration = 0.5f){
		Vector3 start = t.position;
		Vector3 dir = direction;
		Vector3 tip = start + dir;
		Debug.DrawLine(t.position, tip, color, duration);
	}
	
	// Draw an arrow from transform in the direction specified
	public static void DrawArrow(Transform t, Vector2 direction, float duration = 0.5f){
		Vector3 start = t.position;
		Vector3 dir = direction;
		Vector3 tip = t.position + dir;
		Vector2 tip2 = tip;
		Debug.DrawLine(t.position, tip, Color.red, 0.5f);
		Debug.DrawLine(tip, (tip2 + Math2d.RotVector(dir, 135f)/2), Color.red, duration);
		Debug.DrawLine(tip, (tip2 + Math2d.RotVector(dir, -135f)/2), Color.red, duration);
	}
	
	// Draw an arrow from transform in the direction specified
	public static void DrawArrow(Transform t, Vector2 direction, Color color, float duration = 0.5f){
		Vector3 start = t.position;
		Vector3 dir = direction;
		Vector3 tip = t.position + dir;
		Vector2 tip2 = tip;
		Debug.DrawLine(t.position, tip, color, 0.5f);
		Debug.DrawLine(tip, (tip2 + Math2d.RotVector(dir, 135f)/2), color, duration);
		Debug.DrawLine(tip, (tip2 + Math2d.RotVector(dir, -135f)/2), color, duration);
	}
}
