using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Count : MonoBehaviour
{
    private int EnemyCount;
    private ObjectTeleportation teleportScript;
    private bool allEnemiesDead;
    private int ID;
    // Start is called before the first frame update
    void Start()
    {
        EnemyCount = 0;
    }
    private void Update()
    {
        if(EnemyCount == 0)
        {
            allEnemiesDead = true;
            //Call ObjectTeleportation and change the value to true...
        }
    }

    private void Awake()
    {
        GameObject room = GameObject.Find("Room " + ID);
        List<GameObject> portals = new List<GameObject>();

        // Traverse through all children of the room
        foreach (Transform child in room.transform)
        {
            // Check if the child's name matches "Portal"
            if (child != null && (child.gameObject.name == "Portal" || child.gameObject.name == "Portal(Clone)"))
            {
               portals.Add(child.gameObject);
            }
        }
       Debug.Log(portals.Count);
    }

    public void setEnemyCount(int amount)
    {
        EnemyCount = amount;
    }
    
    public int getEnemyCount()
    {
        return EnemyCount;
    }
    
    public void setId(int id)
    {
        ID = id;
    }

    public int getId()
    {
        return ID;
    }
}
