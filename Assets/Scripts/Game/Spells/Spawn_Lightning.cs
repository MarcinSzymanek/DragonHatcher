using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public sealed class Spawn_Lightning : SpellBase
{
    public GameObject lightningPrefab;
    public LayerMask targetLayer;
    public float spawnRadius = 5f;
	
	internal override void onCast(){
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
