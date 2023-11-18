using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellLogic : MonoBehaviour
{
	//public Spell spell;
	public UnityEvent<VectorTarget> onCast;
	public int id;
	public int castDelay;
	private float castDelay_;
	private VectorTarget target_;
	
	private void Start(){
		castDelay_ = castDelay/1000f;
	}
	
	public void CastSpell(VectorTarget target){
		target_ = target;
		Invoke("cast_", castDelay_);
	}
	
	private void cast_(){
		onCast.Invoke(target_);
	}
}
