using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ResourceID{
    wood,
	stone,
	fire,
	air,
    gold
}
[CreateAssetMenu(fileName="Resource", menuName = "ScriptableObjects/Resource")]
[Serializable]
public class ResourceSO : ScriptableObject
{
    public string Name;
    public string Icon;
    public string SpritePickable;
	public ResourceID ID;
	[SerializeField]
	public int Tier;
}

