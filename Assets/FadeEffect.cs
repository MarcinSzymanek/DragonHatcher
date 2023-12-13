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
	
	void Awake(){
		image_ = GetComponent<Image>();	
	}
	
	void Start(){
		ScreenFadeIn();
	}
	
	public void ScreenFadeIn(){
		StartCoroutine(fadeIn());
	}
	
	public void ScreenFadeOut(){
		StartCoroutine(fadeOut());
	}
	
	IEnumerator fadeOut(){
		// This is necessary, cause otherwise the overlay will block UI buttons and elements
		image_.rectTransform.localScale = new Vector3(1, 1, 1);
		float step = fadeinTime/100;
		while(image_.color.a < 1){
			image_.color = new Color(0, 0, 0, image_.color.a + 0.01f);
			yield return new WaitForSeconds(step);
		}
		fadeOutFinished?.Invoke();
	}
	
	IEnumerator fadeIn(){
		float step = fadeinTime/100;
		while(image_.color.a > 0){
			image_.color = new Color(0, 0, 0, image_.color.a - 0.01f);
			yield return new WaitForSeconds(step);
		}
		// This is necessary, cause otherwise the overlay will block UI buttons and elements from being interacted with
		image_.rectTransform.localScale = new Vector3(0, 0, 0);
		fadeInFinished?.Invoke();
	}
	
	
	
}
