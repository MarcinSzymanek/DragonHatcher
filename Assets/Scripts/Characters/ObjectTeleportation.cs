using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTeleportation : MonoBehaviour
{
    public Transform objectToTeleport, destination;
    public GameObject playerg;

    void OnTriggerEnter2D(Collider2D other) {
        if(other != null) {
            playerg.SetActive(false);
            objectToTeleport.position = destination.position;
            playerg.SetActive(true);
        }
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
