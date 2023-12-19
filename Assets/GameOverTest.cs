using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	    Invoke("SwitchToMain", 1f);
    }
    
	void SwitchToMain(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");		
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
