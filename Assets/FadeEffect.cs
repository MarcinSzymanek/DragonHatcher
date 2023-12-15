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
	public bool fadeInRunning = false;
	
	void Awake(){
		image_ = GetComponent<Image>();	
	}
	
	void Start(){
		ScreenFadeIn();
	}
	
	public void ScreenFadeIn(System.Action? onFinish = null){
		StartCoroutine(fadeIn(onFinish));
	}
	
	public void ScreenFadeOut(System.Action? onFinish = null){
		StartCoroutine(fadeOut(onFinish));
	}
	
	IEnumerator fadeOut(System.Action? onFinish = null){
		// Wait for fadein to finish if needed
		while(fadeInRunning){
			yield return null;
		}
		
		image_ = GetComponent<Image>();
		// This is necessary, cause otherwise the overlay will block UI buttons and elements
		Debug.Log("Fading ouuuuut");
		transform.localScale = Vector3.one;
		image_.rectTransform.localScale = Vector3.one;
		float step = fadeinTime/50;
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
	}
	
	IEnumerator fadeIn(System.Action? onFinish = null){
		fadeInRunning = true;
		image_.rectTransform.localScale = new Vector3(1, 1, 1);
		float step = fadeinTime/100;
		while(image_.color.a > 0){
			image_.color = new Color(0, 0, 0, image_.color.a - 0.01f);
			yield return new WaitForSeconds(step);
		}
		// This is necessary, cause otherwise the overlay will block UI buttons and elements from being interacted with
		Debug.Log("Resetting rectTransform :)");
		image_.rectTransform.localScale = new Vector3(0, 0, 0);
		fadeInFinished?.Invoke();
		onFinish?.Invoke();
		fadeInRunning = false;
	}
	
	
	
}
