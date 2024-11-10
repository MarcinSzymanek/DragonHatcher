using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
	
	public static UIManager Instance;
	private IUIGroup mainMenu_;
	[SerializeField]
	private IUIGroup gameUI_;
	private GameObject mainMenuGO_;
	
    void Awake()
    {
	    if(Instance != null) Destroy(this);
	    Instance = this;
	    Debug.Log("UIManager instance activated");
    }
    
	public void OnGameStart(){
		mainMenu_.FadeOut();
		StartCoroutine(Utils.Enumerators.DoAfter(
			() => {
				gameUI_.FadeIn();
				Destroy(mainMenuGO_);
			},
			2f
		));
	}
	
	
	
	public void Register(IUIGroup uiElement)
	{
		switch (uiElement.GetType())
		{
		case UIType.GAME_UI:
			gameUI_ = uiElement;
			break;

		case UIType.MAIN_MENU:
			mainMenu_ = uiElement;
			mainMenuGO_ = (mainMenu_ as UIMainMenu).gameObject;
			break;	
		}
	}

	public void Remove(IUIGroup uiElement)
	{
		switch (uiElement.GetType())
		{
		case UIType.GAME_UI:
			gameUI_ = null;
			break;

		case UIType.MAIN_MENU:
			Debug.Log("Removing Main Menu", this);
			mainMenu_ = null;
			break;	
		}
	}
}
