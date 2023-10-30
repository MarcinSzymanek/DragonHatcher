using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
	GameObject Spawn(int idx, Vector3 position);
	GameObject Spawn(int idx, Vector3 position, Transform parent);
}
