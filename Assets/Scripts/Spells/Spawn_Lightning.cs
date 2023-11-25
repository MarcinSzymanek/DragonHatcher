using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Lightning : MonoBehaviour, ISpell
{
    public GameObject lightningPrefab;
    public LayerMask targetLayer;
    public float spawnRadius = 5f;
    public int id{get; set;}
	string name_;
	public string name{get => name_;}

    public void CastSpell(SpellParameters parameters) {
        SpawnLightningAboveEnemies();
    }

    public void SpawnLightningAboveEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, spawnRadius, targetLayer);
        
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Debug.Log("hey");
            Vector3 spawnPosition = enemyCollider.transform.position + Vector3.up;
            GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
