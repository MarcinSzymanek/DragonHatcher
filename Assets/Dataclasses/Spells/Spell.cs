using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

// This dataclass is used to store data about spells which does not quite fit into the spell prefab
// Field slot: hotkey number of the spells. If not assigned, returns -1
[Serializable]
[CreateAssetMenu(fileName = "New spell", menuName = "Spell")]
public class SpellDataObject : ScriptableObject
{
	public int slot = -1;
	public string name;
	public string description;
	[SerializeReference]
	public Sprite icon;
	public float cooldown;
	[SerializeReference]
	public GameObject spellPrefab;
	[SerializeReference]
	public LayerMask target;
}
