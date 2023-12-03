using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

// This dataclass is used to store data about spells which does not quite fit into the spell prefab
[Serializable]
[CreateAssetMenu(fileName = "New spell", menuName = "Spell")]
public class SpellDataObject : ScriptableObject
{
	public string name;
	public string description;
	public Sprite icon;
	public float cooldown;
	[SerializeReference]
	public GameObject spellPrefab;
	public LayerMask target;
}
