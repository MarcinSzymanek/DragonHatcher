using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void Play() 
    {
        SceneManager.LoadScene("WaveDefense");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
