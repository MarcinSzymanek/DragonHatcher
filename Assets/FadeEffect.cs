using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
	Image image_;
	[field: SerializeField]
	public float fadeinTime{get; set;} 
	public event System.Action fadeInFinished;
	public event System.Action fadeOutFinished;
	
	// We need to keep track if any fade effect is in progress and only have one active at a time
	// Otherwise we get to situations where both effects are trying to adjust alpha colour at the same time and bugs happen
	public bool effectActive = false;
	
	void Awake(){
		image_ = GetComponent<Image>();	
	}
	
	void Start(){
		ScreenFadeIn();
	}
	
	// This method is asynchronous. Use onFinish param as callback. Waits for ScreenFadeOut to finish before proceeding
	// duration is optional and overrides the object fade duration. Must be positive to have effect.
	public void ScreenFadeIn(System.Action? onFinish = null, float duration = -1f){
		StartCoroutine(fadeIn(duration, onFinish));
	}
	
	
	// This method is asynchronous. Use onFinish param as callback. Waits for ScreenFadeIn to finish before proceeding
	// duration is optional and overrides the object fade duration. Must be positive to have effect
	public void ScreenFadeOut(System.Action? onFinish = null, float duration = -1f){
		StartCoroutine(fadeOut(duration, onFinish));
	}
	
	IEnumerator fadeOut(float duration, System.Action? onFinish = null){
		float fadeDuration = fadeinTime;
		if(duration > 0){
			fadeDuration = duration;
		}
		
		// Wait for fadein to finish if needed
		while(effectActive){
			yield return null;
		}
		effectActive = true;
		
		image_ = GetComponent<Image>();
		transform.localScale = Vector3.one;
		image_.rectTransform.localScale = Vector3.one;
		float step = fadeDuration/50;
		while(image_.color.a < 0.5){
			image_.color = new Color(0, 0, 0, image_.color.a + 0.02f);
			yield return new WaitForSeconds(step);
		}
		while(image_.color.a < 1){
			image_.color = new Color(0, 0, 0, image_.color.a + 0.05f);
			yield return new WaitForSeconds(step);
		}
		fadeOutFinished?.Invoke();
		onFinish?.Invoke();
		effectActive = false;
	}
	
	IEnumerator fadeIn(float duration, System.Action? onFinish = null){
		float fadeDuration = fadeinTime;
		if(duration > 0){
			fadeDuration = duration;
		}

		// Wait for fadeout to finish if needed
		while(effectActive){
			yield return null;
		}
		effectActive = true;
		image_.rectTransform.localScale = new Vector3(1, 1, 1);
		float step = fadeDuration/100;
		while(image_.color.a > 0){
			image_.color = new Color(0, 0, 0, image_.color.a - 0.01f);
			yield return new WaitForSeconds(step);
		}
		// This is necessary, cause otherwise the overlay will block UI buttons and elements from being interacted with
		image_.rectTransform.localScale = new Vector3(0, 0, 0);
		fadeInFinished?.Invoke();
		onFinish?.Invoke();
		effectActive = false;
	}
	
	
	
}
