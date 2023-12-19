using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    Transform tf_;
	Vector3 pos_;
	Camera cam_;
    // Start is called before the first frame update
    void Start()
	{
		UnityEngine.Cursor.visible = false;
	    tf_ = GetComponent<Transform>();
	    cam_ = Camera.main;
    }

    // Follow mouse position
    void Update()
    {
        updatePosition();
    }

    void updatePosition(){
	    var mousePos = cam_.ScreenToWorldPoint(Input.mousePosition);
		
        tf_.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
    
	public Vector3 GetPosition(){
		return pos_;
	}
}
