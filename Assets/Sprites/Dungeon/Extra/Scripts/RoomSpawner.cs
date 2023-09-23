using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //Define a value that will decide what room will be taken, 1 for top, 2 for bottom etc...
    public int openingDirection;
    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    void Start() {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>(); 
        Invoke("Spawn", 0.1f);
    }



    void Spawn() {
        if(!spawned) {
            //This can 100% be optimized and made looked way better
            if(openingDirection == 1) {
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            if(openingDirection == 2) {
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            if(openingDirection == 3) {
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            if(openingDirection == 4) {
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            spawned = true;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("SpawnPoint")) {
            Destroy(gameObject);
        }
    }
}
