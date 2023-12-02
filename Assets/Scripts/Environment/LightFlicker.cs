using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightFlicker : MonoBehaviour
{
	UnityEngine.Rendering.Universal.Light2D light_;
    
	private Color defaultColor;
	private float defaultIntensity;
	private float intensityStep = 0.5f;
	private float defaultOuterRadius;
	private float outerRadiusStep = 0.4f;
	private bool stepUp = false;
	private int counter = 0;
    
	void Awake()
    {
	    light_ = transform.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
	    defaultColor = light_.color;
	    defaultIntensity = light_.intensity;
    }
    
	void FixedUpdate(){
		OnAnimationStep();
	}
    
	private void OnAnimationStep(){
		counter += 1;
		
		if(counter < 30) return;
		light_.intensity += (stepUp)? intensityStep: -intensityStep;
		light_.pointLightOuterRadius += (stepUp)? outerRadiusStep: -outerRadiusStep;
		if(stepUp) stepUp = false;
		else stepUp = true;
		
		counter = 0;
		
		
		
	} 
	
}
