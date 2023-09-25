using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIResource : MonoBehaviour
{
	TextMeshProUGUI text_comp_;
	public ResourceID ID;
    void Awake()
    {
	    text_comp_ = GetComponentInChildren<TextMeshProUGUI>();
    }
    
	public void UpdateText(string text){
		text_comp_.text = text;
	}

}
