using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonEvents : MonoBehaviour
{
	AudioSource button_click;
    // Start is called before the first frame update
    void Start()
    {
	    button_click = GetComponent<AudioSource>();
    }
    
	public void PlaySound(){
		button_click.PlayOneShot(button_click.clip);
	}
}
