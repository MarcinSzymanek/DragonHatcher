using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool GameIsPaused = false;
    UISettingsMenu settings;
	GameObject pauseObject;
    
    private void Awake()
	{
		pauseObject = transform.Find("UIPauseMenu").gameObject;
		Debug.Log(pauseObject.name);
	    settings = GetComponentInChildren<UISettingsMenu>(true);
	    Debug.Log(settings.name);
    }

    public void PauseLogic()
	{
        if (settings.gameObject.activeSelf)
        {
            settings.gameObject.SetActive(false);
	        pauseObject.SetActive(true);
	        return;
        }
        if (GameIsPaused)
        {
	        Resume();
	        return;
        }
        Pause();
    }

    public void Resume()
    {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
