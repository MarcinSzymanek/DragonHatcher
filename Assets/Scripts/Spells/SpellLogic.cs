using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellLogic : MonoBehaviour
{
	//public Spell spell;
	public UnityEvent onCast;
	public int id;
	public int castDelay;
	private float castDelay_;
	
	private void Start(){
		castDelay_ = castDelay/1000f;
	}
	
	public void CastSpell(){
		Invoke("cast_", castDelay_);
	}
	
	private void cast_(){
		onCast.Invoke();
	}
}
