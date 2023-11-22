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

    private void Awake()
    {
        attachedGameObject = this.gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && canTeleport)
        {
            StartCoroutine(TeleportPlayer());
            canTeleport = false;
        }
    }

    IEnumerator TeleportPlayer()
    {
        playerg.SetActive(false);

        yield return new WaitForSeconds(teleportDelay);

        Vector3 teleportOffset = Vector3.zero;

        // Check the player's movement direction relative to the teleporter so we dont mess up the teleportation offset
        if (playerg.transform.position.y < gameObject.transform.position.y)
        {
            teleportOffset = new Vector3(0f, 2f, 0f);
        }
        else if (playerg.transform.position.y > gameObject.transform.position.y && (playerg.transform.position.x < gameObject.transform.position.x || playerg.transform.position.x > gameObject.transform.position.x))
        { 
            teleportOffset = new Vector3(0f, -2f, 0f);
        }

        // Apply the calculated offset to the teleport destination
        objectToTeleport.position = destination.position + teleportOffset;

        playerg.SetActive(true);
        yield return new WaitForSeconds(0.5f);
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
