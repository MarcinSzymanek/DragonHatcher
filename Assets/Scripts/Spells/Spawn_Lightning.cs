using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Spawn_Lightning : MonoBehaviour, ISpell
{
    public GameObject lightningPrefab;
    public LayerMask targetLayer;
    public float spawnRadius = 5f;
    public int id {get; set;}
 
    public void CastSpell(SpellParameters parameters) {
        SpawnLightningAboveEnemies();
    }
	
	public void CastSpell(Vector3 mousePos){
		SpawnLightningAboveEnemies();
	}
	
    public void SpawnLightningAboveEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, spawnRadius, targetLayer);
        
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Vector3 spawnPosition = enemyCollider.transform.position;
            GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
