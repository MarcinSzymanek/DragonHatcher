using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool GameIsPaused = false;
    UISettingsMenu settings;
    PauseMenu pause;

    private void Awake()
    {
        settings = GetComponentInChildren<UISettingsMenu>();
        pause = GetComponent<PauseMenu>();
    }

    public void PauseLogic()
    {
        //Debug.Log(settings.name);
        //Debug.Log(pause.name);
        //if (settings.gameObject.activeSelf)
        //{
            //settings.gameObject.SetActive(false);
            //pause.gameObject.SetActive(false);
        //}
        if (GameIsPaused)
        {
            Resume();
        }
        if (!GameIsPaused)
        {
            Pause();
        }
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
