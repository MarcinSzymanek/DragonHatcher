using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public sealed class Spawn_Lightning : SpellBase<Int32>
{
    public GameObject lightningPrefab;
    public LayerMask targetLayer;
    public float spawnRadius = 5f;
	
	internal override Int32 getTarget(Vector3 mousePos){
		return 0;
	}
	
	internal override void onCast(Int32 nothing){
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
