using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIGroupFadeEffects : MonoBehaviour
{
	const float DEFAULT_SPEED = 0.1f;
	const float DEFAULT_DURATION = 1f;
	CanvasGroup canvasGroup_;

    void Awake()
    {
	    canvasGroup_ = GetComponent<CanvasGroup>();	
    }
    

	private void fadeIn(float duration, float speed)
	{
		float increment = 1/(duration/speed);
		StartCoroutine(Utils.Enumerators.DoUntilAndThen(
			() => {
				canvasGroup_.alpha += increment;
			},
			() => canvasGroup_.alpha >= 1f,
			() => {
				canvasGroup_.alpha = 1f;
			},
			speed
		));
	}
	
	private void fadeOut(float duration, float speed)
	{
		float decrement = 1/(duration/speed);
		StartCoroutine(Utils.Enumerators.DoUntilAndThen(
			() => {
				canvasGroup_.alpha -= decrement;
			},
			() => canvasGroup_.alpha <= 0f,
			() => {
				canvasGroup_.alpha = 0f;
			},
			speed
		));
	}
	
	
	public void FadeIn()
	{
		fadeIn(DEFAULT_DURATION, DEFAULT_SPEED);
	}
	
	public void FadeIn(float duration, float speed)
	{
		fadeIn(duration, speed);
	}
	
	public void FadeOut()
	{
		fadeOut(DEFAULT_DURATION, DEFAULT_SPEED);
	}
	
	public void FadeOut(float duration, float speed)
	{
		fadeOut(duration, speed);
	}

}
