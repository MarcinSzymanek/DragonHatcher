using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New building", menuName = "Building")]
public class BuildingData : ScriptableObject
{
	public List<ResourceCost> cost;
	public GameObject prefab;
	public Sprite icon;
}

[Serializable]
public class ResourceCost{
	public ResourceID id;
	public int amount;
}
