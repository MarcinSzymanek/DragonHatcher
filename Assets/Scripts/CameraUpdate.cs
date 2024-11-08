using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpdate : MonoBehaviour
{
	/*
	* This script should position the object between
	* Player and Cursor - for the camera
	*/
	Transform player_;
	Transform cursor_;
    // Start is called before the first frame update
    void Start()
    {
    	player_ = GameObject.Find("Player").transform;
    	cursor_ = GameObject.Find("Cursor").transform;
    }

    // Update is called once per frame
	void FixedUpdate()
	{
		float x = player_.position.x + (cursor_.position.x - player_.position.x)/3;
		float y = player_.position.y + (cursor_.position.y - player_.position.y)/3;
		transform.position = new Vector3(x, y, -9);
    }
}
