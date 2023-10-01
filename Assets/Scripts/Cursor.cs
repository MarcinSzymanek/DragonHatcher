using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public Camera camera;
public Transform obj;

Vector2 objPos;
Vector2 mousePos;
float mousePosY, mousePosX;

public class Cursor : MonoBehaviour
{
  

    // Update is called once per frame
    void Update()
    {
    objPos = obj.transform.position;//gets player position
	mousePos = Input.mousePosition;//gets mouse postion
	mousePos = camera.ScreenToWorldPoint (mousePos);
	mousePosX = mousePos.x - objPos.x;//gets the distance between object and mouse position for x
	mousePosY = mousePos.y - objPos.y;//gets the distance between object and mouse position for y  if you want this.
    }
}
