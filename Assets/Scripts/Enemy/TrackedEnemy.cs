using UnityEngine;

// This enemy is tracked by EnemyTracker
public class TrackedEnemy : MonoBehaviour
{
	private void Awake()
	{
		EnemyTracker.Instance.RegisterEnemySpawned(this.gameObject);
		GetComponent<DeathController>().objectDied += onDeath;
	}

	private void onDeath(object? _, ObjectDeathArgs dontcare)
	{
		EnemyTracker.Instance.RegisterEnemyDied(this.gameObject);
	}
}