using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastDelayIndicator : MonoBehaviour
{
	UnityEngine.Rendering.Universal.Light2D light_;
	RectTransform tf_;
	Image image_;
	float startX_;
	public float defaultOffset = 60f;
	public float minIntensity = 10f;
	public float minR = 0;
	public float minG = 0;
	public float minB = 100;
	public float maxRB = 255f;
	
	void Awake(){
		tf_ = GetComponent<RectTransform>();
		light_ = GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();	
		image_ = GetComponent<Image>();	
		startX_ = tf_.localPosition.x;
	}
	
	public void OnCast(float castDelay){
		int noSteps;
		if(castDelay < 0.5){
			noSteps = 5;
		}
		else if(castDelay < 1.0){
			noSteps = 10;
		}
		else{
			noSteps = 25;
		}
		try {
			
			StartCoroutine(moveIndicator(noSteps, castDelay));
		}
			catch(MissingReferenceException missingRef){
				Debug.Log("Tried to use castdelay indicator but what the fuck");
			}
		
	}
	
	IEnumerator moveIndicator(int noSteps, float castDelay){

		light_.enabled = true;
		float stepScale = castDelay/noSteps;
		int currentStep = 0;
		int halfstep = noSteps/2;
		float diffR = 255f - minR;
		float diffG = 255f - minG;
		float diffB = 255f - minB;
		image_.color = new Color(1, 1, 1, 70/255f);
		while(currentStep < halfstep){
			currentStep++;
			tf_.localPosition = new Vector3(startX_ + (currentStep * (60/noSteps)), 0, 0);
			// Add up to 50 intensity
			//light_.intensity += (stepScale * 50f);
			//light_.color = new Color(
			//	minR + (currentStep * stepScale * diffR), 
			//	minG + (currentStep * stepScale * diffG),
			//	minB + (currentStep* stepScale * 2 * diffB));
			// Add up to 60 alpha color
			
			yield return new WaitForSeconds(stepScale);
		}
		while(currentStep < noSteps){
			currentStep++;
			tf_.localPosition = new Vector3(startX_ + (currentStep * (60/noSteps)), 0, 0);
			//light_.intensity += (stepScale * 50f);
			//light_.color = new Color(
			//	minR + (currentStep * stepScale * diffR), 
			//	minG + (currentStep * stepScale * diffG),
			//	255f);
			//image_.color = new Color(1, 1, 1, currentStep * stepScale * 60/255f);
			yield return new WaitForSeconds(stepScale);
		}
		// Reset
		light_.enabled = false;
		light_.color = new Color(minR, minG, minB);
		light_.intensity = minIntensity;
		image_.color = new Color(255, 255, 255, 0);
		tf_.localPosition = new Vector3(startX_, tf_.localPosition.y, tf_.localPosition.z);
	}

    
}
