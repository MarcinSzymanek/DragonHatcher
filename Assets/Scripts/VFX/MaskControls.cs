using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskControls : MonoBehaviour
{
	Transform tf_;
    void Start()
    {
	    tf_ = transform;
    }
	
	public void MoveMask(float x, float y){
		tf_.localPosition = new Vector3(x, y, transform.position.z);
	}
}
