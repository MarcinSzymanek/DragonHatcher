using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIGroupFadeEffects))]
public class UIGame : MonoBehaviour, IUIGroup
{
	
	const float DEFAULT_SPEED = 0.01f;
	const float DEFAULT_DURATION = 1f;
	UIGroupFadeEffects fadeEffects_;

	void Awake()
	{
		fadeEffects_ = GetComponent<UIGroupFadeEffects>();
	}
	
	void Start()
	{
		UIManager.Instance.Register(this);
	}
	
	void OnDestroy()
	{
		UIManager.Instance.Remove(this);
	}
	
	public UIType GetType() => UIType.GAME_UI;
	
	
	public void FadeIn()
	{
		fadeEffects_.FadeIn();
	}
	
	public void FadeOut()
	{
		fadeEffects_.FadeOut();
		
	}
	
	public void FadeOut(float duration, float speed)
	{
		fadeEffects_.FadeOut(duration, speed);
	}

	public void FadeIn(float duration, float speed)
	{
		fadeEffects_.FadeIn(duration, speed);
	}
	
}
