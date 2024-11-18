using UnityEngine;
using System.Collections.Generic;

// Track enemies spawning in and dying. Their colliders need to be stored so spells that use particle collision
// always know which colliders they can interact with. Track possible targets for the AI
public class EnemyTracker : MonoBehaviour
{
	private List<GameObject> enemiesAlive_ = new List<GameObject>();
	private List<GameObject> aiTargets_ = new List<GameObject>();

	public event System.Action<GameObject> EnemySpawnedEvent;
	public event System.Action<GameObject> EnemyDiedEvent;
	
	public static EnemyTracker Instance {get; private set;}
	
	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
    
	public void RegisterEnemySpawned(GameObject enemy)
	{
		enemiesAlive_.Add(enemy);
		EnemySpawnedEvent?.Invoke(enemy);
	}
	
	public void RegisterEnemyDied(GameObject enemy)
	{
		enemiesAlive_.Remove(enemy);
		EnemyDiedEvent?.Invoke(enemy);
	}
	
	public void RegisterAITarget(GameObject target)
	{
		if(aiTargets_.Contains(target)) return;
		aiTargets_.Add(target);
	}
	
	public void RemoveAITarget(GameObject target)
	{
		if(!aiTargets_.Contains(target)) return;
		aiTargets_.Remove(target);
	}
	
	public List<Collider2D> GetEnemyColliders()
	{
		List<Collider2D> colliderList = new List<Collider2D>();

		foreach(GameObject obj in enemiesAlive_)
		{
			colliderList.Add(obj.transform.Find("Hitbox").GetComponent<Collider2D>());
		}

		return colliderList;
	}

	public GameObject[] GetTargetsInScene()
	{
		return aiTargets_.ToArray();
	}
}
