using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class LightEffects : MonoBehaviour
{
	[Serializable]
	public class Flicker
	{
		public bool enabled;	
		public float intensityStep = 1f;
		public float radiusMult = 2f;
		public float delayBetweenFlicker = 0.5f;
		public float flickerTime = 0.2f;
		public bool stepUp = true;
		//private void OnAnimationStep(){
		//	counter += 1;
		
		//	if(counter < 30) return;
		//	light_.intensity += (stepUp)? intensityStep: -intensityStep;
		//	light_.pointLightOuterRadius += (stepUp)? outerRadiusStep: -outerRadiusStep;
		//	if(stepUp) stepUp = false;
		//	else stepUp = true;
		
		//	counter = 0;
		//} 
	}
	
	[Serializable]
	public class Flash
	{
		public bool enabled;
		public bool use_volumetric_light;
		public float intensityAmp;
		public float falloffSpeedMultiplier;
		
	}
	
	private Light2D light_;
	
	public Color Color;
	public float Intensity;	
	public float OuterRadius;
	
	private float baseIntensity_;
	private float baseInnerRadius_;
	private float baseOuterRadius_;
	
	public Flicker flicker;
	public Flash flash;
	
	void Awake()
	{
		light_ = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
		baseIntensity_ = light_.intensity;
		baseInnerRadius_ = light_.pointLightInnerRadius;
		baseOuterRadius_ = light_.pointLightOuterRadius;
		if(flicker.enabled) Invoke("processFlicker", flicker.delayBetweenFlicker);
	}
	
	void processFlicker(){
		if(!flicker.enabled) return;
		
		if(flicker.stepUp){
			light_.intensity += flicker.intensityStep;
			light_.pointLightInnerRadius *= flicker.radiusMult;
			light_.pointLightOuterRadius *= flicker.radiusMult;
			flicker.stepUp = false;
			Invoke("processFlicker", flicker.delayBetweenFlicker);
		}
		else{
			light_.intensity -= flicker.intensityStep;
			light_.pointLightInnerRadius = baseInnerRadius_;
			light_.pointLightOuterRadius = baseOuterRadius_;
			flicker.stepUp = true;
			Invoke("processFlicker", flicker.flickerTime);
		}
	}
	

}


