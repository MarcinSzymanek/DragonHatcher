using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(UIGroupFadeEffects))]
public class UIMainMenu : MonoBehaviour, IUIGroup
{
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
	
	public UIType GetUIType() => UIType.MAIN_MENU;
	
	public void FadeIn()
	{
		fadeEffects_.FadeIn();
	}
	
	public void FadeOut()
	{
		fadeEffects_.FadeOut();
		
	}
	
	public void FadeOut(float duration, float speed){
		fadeEffects_.FadeOut(duration, speed);
	}
	public void FadeIn(float duration, float speed){
		fadeEffects_.FadeIn(duration, speed);
	}

    public void Play() 
    {
        SceneManager.LoadScene("WaveDefense");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
