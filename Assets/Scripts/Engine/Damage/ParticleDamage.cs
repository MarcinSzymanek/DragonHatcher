using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This script is for particles that can damage other entities
/// </summary>
public class ParticleDamage : MonoBehaviour
{
	List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
	List<Collider2D> elligibleColliders_ = new List<Collider2D>();

	private ParticleSystem particleSystem_;
	
	private void Start()
	{
		particleSystem_ = GetComponentInChildren<ParticleSystem>();
		elligibleColliders_ = EnemyTracker.Instance.GetEnemyColliders();
		EnemyTracker.Instance.EnemySpawnedEvent += addEnemyCollider;
	}
    
	private void addEnemyCollider(GameObject enemy)
	{
		Collider2D collider = enemy.transform.Find("Hitbox").GetComponent<Collider2D>();
		elligibleColliders_.Add(collider);
		particleSystem_.trigger.AddCollider(collider);	
	}
    
	protected void OnParticleTrigger()
	{
		int numEnter = particleSystem_.GetTriggerParticles(
			ParticleSystemTriggerEventType.Enter,
			enter,
			out ParticleSystem.ColliderData data
		);

		for(int i = 0; i < enter.Count; i++)
		{
			Component coll = data.GetCollider(i, 0);
			TakeDamage dmgScript = coll.GetComponentInParent<TakeDamage>();	
			if(dmgScript is not null)
			{
				dmgScript.TriggerTakeDamage(2);
			}
		}
	}
}
