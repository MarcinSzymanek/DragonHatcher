using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddHoverText : MonoBehaviour
{
	GameObject prefab_;
	
	[SerializeField]
	[TextArea]
	string text_;
	TextMeshPro textComponent_;
    // Start is called before the first frame update
	void Awake()
    {
	    var tf = gameObject.transform;
	    textComponent_ = GetComponentInChildren<TextMeshPro>();
    }
    
	void OnEnable()
	{
		textComponent_.enabled = true;
	}
    
	void OnDisable()
	{
		textComponent_.enabled = false;
		//textComponent_.text = "";
	}
    
	public void SetText(string text) {
		text_ = text;	
		textComponent_.text = text;
	}

}
