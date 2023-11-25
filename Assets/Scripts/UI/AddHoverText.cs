using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddHoverText : MonoBehaviour
{
	GameObject prefab_;
	
	[SerializeField]
	string text_;
	TextMeshPro textComponent_;
    // Start is called before the first frame update
    void Start()
    {
	    var tf = gameObject.transform;
	    prefab_ = Resources.Load("Prefabs/Debug/HoverText") as GameObject;
	    var obj = Instantiate(prefab_, tf);
	    if(obj.TryGetComponent<TextMeshPro>(out TextMeshPro textComp)) {
	    	textComponent_ = textComp;
	    	textComponent_.text = text_;
	    }
    }
    
	public void SetText(string text) {
		textComponent_.text = text;
	}

}
