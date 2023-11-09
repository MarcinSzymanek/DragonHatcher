using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
	public string name;
	public string description;
	public float cooldown;
	public bool projectile;
	public bool groundTargeted;
	public LayerMask target;
	
	public UnityEvent onCast;
}
