using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
	GameObject Spawn(Vector3 position);
	GameObject Spawn(Vector3 position, Transform parent);
}
