using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathVisualise : MonoBehaviour
{
	// Draw an arrow from transform in the direction specified
	public static void DrawArrow(Transform t, Vector2 direction){
		Vector3 start = t.position;
		Vector3 dir = direction;
		Vector3 tip = t.position + dir;
		Vector2 tip2 = tip;
		Debug.DrawLine(t.position, tip, Color.red, 0.5f);
		Debug.DrawLine(tip, (tip2 + Math2d.RotVector(dir, 135f)/2), Color.red, 0.5f);
		Debug.DrawLine(tip, (tip2 + Math2d.RotVector(dir, -135f)/2), Color.red, 0.5f);
	}
}
