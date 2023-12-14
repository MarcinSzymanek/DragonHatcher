using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUIComponents : MonoBehaviour
{
	public void DisableUI(){
		Debug.Log("Call disableui");
		var children = GetComponentsInChildren<Transform>();
		foreach(var child in children){
			if(child.GetComponent<FadeEffect>() != null || child == transform){
				continue;
			}
			child.gameObject.SetActive(false);
		}
	}
}
