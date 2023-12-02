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
		public float intensityStep = 1;
		public int frequency = 2;
    
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
		var a = flash.enabled;
	}
	
}
