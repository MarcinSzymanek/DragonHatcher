using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Lightning : MonoBehaviour
{
    public GameObject lightningPrefab;
    public LayerMask targetLayer;
    public float spawnRadius = 5f;

    public void SpawnLightningAboveEnemies(VectorTarget target)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, spawnRadius, targetLayer);
        
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            // Spawn lightning above each enemy within the radius
            Vector3 spawnPosition = enemyCollider.transform.position + Vector3.up * /* Adjust Y offset */ 2f;
            GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
