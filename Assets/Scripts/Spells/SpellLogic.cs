using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellLogic : MonoBehaviour
{
	//public Spell spell;
	public UnityEvent onCast;
	public int id;

	public void CastSpell(){
		onCast.Invoke();
	}
}
