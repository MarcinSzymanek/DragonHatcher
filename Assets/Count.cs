using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Count : MonoBehaviour
{
    public int EnemyCount;
    private bool allEnemiesDead;
    public int ID;
    public List<ObjectTeleportation> listOfScripts;
    
	// Callback on enemy death
	public void OnEnemyDeath(object? obj, ObjectDeathArgs args){
		EnemyCount--;
		if(EnemyCount < 1){
			foreach(var tpScript in listOfScripts){
				tpScript.EnableTeleportation();
			}
		}
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
	public void AddPortalScript(ObjectTeleportation script)
	{
		if(listOfScripts == null) listOfScripts = new List<ObjectTeleportation>();
        listOfScripts.Add(script);
    }
}
