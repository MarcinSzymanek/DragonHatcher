using UnityEngine;

public enum UIType
{
	MAIN_MENU,
	GAME_UI
}

// Implements fading for a group of UI elements
public interface IUIGroup
{
	void FadeIn();
	// :param duration: Duration in seconds
	// :param speed: How often the screen should update in seconds. Default 0.1s;
	void FadeIn(float duration, float speed);
	void FadeOut();
	// :param duration: Duration in seconds
	// :param speed: How often the screen should update in seconds. Default 0.1s;
	void FadeOut(float duration, float speed);
	UIType GetUIType();
}
