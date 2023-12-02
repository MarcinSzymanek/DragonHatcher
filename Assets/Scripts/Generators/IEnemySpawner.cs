using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIStrategies;

public interface IEnemySpawner
{
	public GameObject Spawn();
	public GameObject Spawn(int index);
	public void SetAIStrategy(IAI_Strategy strategy);
}
