using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightFlicker : MonoBehaviour
{
	UnityEngine.Rendering.Universal.Light2D light_;
    
	private Color defaultColor;
	private float defaultIntensity;
	private float intensityStep = 0.1f;
	private float defaultOuterRadius;
	private float outerRadiusStep = 0.2f;
	private bool stepUp = false;
	private int counter = 0;
    
	void Awake()
    {
	    light_ = transform.GetChild(0).GetComponent<UnityEngine.Rendering.Universal.Light2D>();
	    defaultColor = light_.color;
	    defaultIntensity = light_.intensity;
    }
    
	private void OnAnimationStep(){
		counter += 1;
		
		if(counter < 3) return;
		light_.intensity += (stepUp)? intensityStep: -intensityStep;
		light_.pointLightOuterRadius += (stepUp)? outerRadiusStep: -outerRadiusStep;
		if(stepUp) stepUp = false;
		else stepUp = true;
		if(counter > 3){
			counter = 0;
		}
		
		
	} 
	
}
