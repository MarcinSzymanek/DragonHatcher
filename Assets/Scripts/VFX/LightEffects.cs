using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

[RequireComponent(typeof(Light2D))]
public class LightEffects : MonoBehaviour
{
	void FixedUpdate()
	{
		if(gradual.active) processGradual();
	}

	// Gradual change of the lightning. Runs every FixedUpdate() step
	[Serializable]
	public class Gradual
	{
		public bool enabled;
		public bool active = false;
		public bool repeat = true;
		public bool invertOnMax = true;
		// Duration of time that light increases/decreases over
		public float duration = 1f;
		public float intensityStep;
		public float maxIntensity = 1.0f;
		public float minIntensity = 0f;
		public bool stepUp = true;
		public int ticksLeft = 0;
	}
	
	[Serializable]
	public class Flicker
	{
		public bool enabled;	
		public float intensityStep = 1f;
		public float radiusMult = 2f;
		public float delayBetweenFlicker = 0.5f;
		public float flickerTime = 0.2f;
		public bool stepUp = true;

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
	public Gradual gradual;
	
	void Awake()
	{
		light_ = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
		baseIntensity_ = light_.intensity;
		baseInnerRadius_ = light_.pointLightInnerRadius;
		baseOuterRadius_ = light_.pointLightOuterRadius;
		if(flicker.enabled) Invoke("processFlicker", flicker.delayBetweenFlicker);
		if(gradual.enabled) startGradual();
	}
	
	void startGradual(){
		gradual.intensityStep = 1/(gradual.duration*100)*(gradual.maxIntensity - gradual.minIntensity);
		gradual.ticksLeft = (int)(gradual.duration/gradual.intensityStep);
		gradual.active = true;
	}
	
	void processGradual(){
		if(!gradual.enabled) return;
		if(gradual.ticksLeft <= 0){
			gradual.stepUp = !gradual.stepUp;
			gradual.ticksLeft = (int)(gradual.duration/gradual.intensityStep);	
		}
		if(gradual.stepUp) light_.intensity += gradual.intensityStep;
		else light_.intensity -= gradual.intensityStep;
		gradual.ticksLeft--;
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


