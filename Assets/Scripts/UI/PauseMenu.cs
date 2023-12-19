using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
	public bool GameIsPaused = false;
    UISettingsMenu settings;
	GameObject pauseObject;
    
	public event System.Action resumed;
    
    private void Awake()
	{
		pauseObject = transform.Find("UIPauseMenu").gameObject;
	    settings = GetComponentInChildren<UISettingsMenu>(true);
		GameObject.FindObjectOfType<InputManager>().PauseCreated(this);
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
	    resumed?.Invoke();
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
