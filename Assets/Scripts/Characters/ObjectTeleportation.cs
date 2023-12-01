using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTeleportation : MonoBehaviour
{
    public Transform objectToTeleport, destination;
    public GameObject playerg;
    public float teleportDelay = 1.5f;
    private bool canTeleport = true;
    private GameObject attachedGameObject = null;
    private Vector3 teleportOffset;

    private void Awake()
    {
        teleportOffset = new Vector3(0f, -3f, 0f);
        attachedGameObject = this.gameObject;  
    }

    void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Trigger Enter");
		if (other != null && canTeleport)
        {
        	Debug.Log("I am teleporter");
            playerg.SetActive(false);
            // Check the player's movement direction relative to the teleporter so we dont mess up the teleportation offset
            // First if = entering the teleporter into the next room from below or the side 
            if (destination.transform.position.y <= gameObject.transform.position.y )
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
}
