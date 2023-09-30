using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIScan : MonoBehaviour
{
	public delegate void ObjectEnteredHandler(object? sender, ObjectEnteredArgs args);
	public event ObjectEnteredHandler objectEntered;
    
    
	protected void OnTriggerEnter2D(Collider2D other)
	{
		objectEntered?.Invoke(this, new ObjectEnteredArgs(other.transform));
	}
}

public class ObjectEnteredArgs : EventArgs
{
	public ObjectEnteredArgs(Transform t){
		T = t;
	}
	public Transform T{get; set;}
}


