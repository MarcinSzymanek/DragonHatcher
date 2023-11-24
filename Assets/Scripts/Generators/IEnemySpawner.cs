using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemySpawner
{
	public GameObject Spawn();
	public GameObject Spawn(int index);
}
