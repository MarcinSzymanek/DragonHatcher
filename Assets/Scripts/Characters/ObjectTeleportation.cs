﻿using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Searcher;
using UnityEngine;

public class ObjectTeleportation : MonoBehaviour
{
    public Transform objectToTeleport, destination;
    public GameObject playerg;
	public float teleportDelay = 1.5f;
    
	// Disable teleportation by default
	private bool canTeleport = false;
    private GameObject attachedGameObject = null;
    private Vector3 teleportOffset;
    private FadeEffect fadeEffect;
    public bool isFinalTeleporter;

    private void Awake()
    {
        teleportOffset = new Vector3(0f, -3f, 0f);
        attachedGameObject = this.gameObject;  
        GameObject uiMain = GameObject.Find("UIMain");
        GameObject blackscreen = uiMain.transform.Find("BlackScreen").gameObject;
        fadeEffect = blackscreen.GetComponent<FadeEffect>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter");
        if (other != null && canTeleport)
        {
            if(isFinalTeleporter)
            {
            	canTeleport = false;
            	var controller = GameObject.FindObjectOfType<GameController>();
                controller.OnWinCondition();
                return;
            }
            fadeEffect.ScreenFadeOut(
                () =>
                {
                    teleport();
                    fadeEffect.ScreenFadeIn(null, 2f);
                },
                0.3f
            );
        }
    }
    
	// One of the rare times where I like to have explicit setter methods
	public void DisableTeleportation(){
		canTeleport = false;
	}

	public void EnableTeleportation(){
		canTeleport = true;
	}

    public void teleport()
    {
        Debug.Log("I am teleporter");
        playerg.SetActive(false);
        // Check the player's movement direction relative to the teleporter so we dont mess up the teleportation offset
        // First if = entering the teleporter into the next room from below or the side 
        if (destination.transform.position.y <= gameObject.transform.position.y)
        {

            teleportOffset = new Vector3(0f, 2f, 0f);
        }
        else if (destination.transform.position.y >= gameObject.transform.position.y)
        {
            teleportOffset = new Vector3(0f, -2f, 0f);
        }
        // 

        disableTpFor(2.0f);
        objectToTeleport.position = destination.position + teleportOffset;
        playerg.SetActive(true);
    }

    //Invoke happens in seconds, meaning after a certain amount of time we allow for teleportation to happen again
    public void disableTpFor(float time) {
        canTeleport = false;
        Invoke("enableTp", time);
    }

    //Is called by the invoke above
    public void enableTp()
    {
        canTeleport = true;
    }

    public void setObjectToTeleport(Transform teleportThisObject) {
        objectToTeleport = teleportThisObject;
    }

    public Transform getObjectToTeleport() {
        return objectToTeleport;
    }

    public Transform getDestination() {
        return destination;
    }

    public void setDestination(Transform newDestination) {
        destination = newDestination;
    }

    public GameObject getPlayer() {
        return playerg;
    }

    public void setPlayer(GameObject player) {
        playerg = player;
    }

    public void setFinalTeleporter()
    {
        isFinalTeleporter = true;
    }

}
