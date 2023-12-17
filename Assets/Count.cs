using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Count : MonoBehaviour
{
    public int EnemyCount;
    private bool allEnemiesDead;
    public int ID;
    public List<ObjectTeleportation> listOfScripts;


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
        GameObject room = this.gameObject;
        List<GameObject> portals = new List<GameObject>();

       
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
    public void addPortalScript(ObjectTeleportation script)
    {
        listOfScripts.Add(script);
    }
}
