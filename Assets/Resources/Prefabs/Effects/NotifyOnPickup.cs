using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyOnPickup : MonoBehaviour
{
	public event System.Action pickup;
	
	public void OnPickup(){
		pickup?.Invoke();
	}
}
