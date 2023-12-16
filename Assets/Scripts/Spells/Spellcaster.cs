using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellCastArgs : EventArgs{
	public int Slot{get;}
	public float CastDelay{get;}
	public float Cooldown{get;}
	public SpellCastArgs(int slot, float castDelay, float cooldown){
		Slot = slot;
		CastDelay = castDelay;
		Cooldown = cooldown;
	}
}

public class Spellcaster : MonoBehaviour
{
	public Spellbook spellbook;
	public int maxSpells;
	public ISpell[] spellSlots;
	Animator anim_;
	
	public int debugSpellCount = 0;
    
	public event EventHandler<SpellCastArgs> spellCastEvent;
    
	// Start is called before the first frame update
    void Start()
	{
		anim_ = GetComponentInChildren<Animator>();
		spellbook = GetComponentInChildren<Spellbook>();
		spellSlots = new ISpell[5];
		for(int i = 0; i < maxSpells; i++){
			spellSlots[i] = spellbook.GetSpellById(i);
			if(spellSlots[i] != null) debugSpellCount++;
		}
	    ReadySpell(0, 0);
    }
    
	public void ReadySpell(int slot, int id){
		Debug.Log("Ready spell called");
		spellSlots[slot] = spellbook.GetSpellById(id);
		debugSpellCount++;
	}
	
	public void CastSpell(int slot, Vector3 mousePosition){
		Debug.Log("CastSpell slot " + slot.ToString());
		#nullable enable
		ISpell? spell = spellbook.GetSpellById(slot);
		if(spell == null){
			Debug.LogWarning("Player tried to cast spell at slot " + slot.ToString() + ", but does not have spell in that slot!");
			return;
		}
		#nullable disable
		//Debug.Log("Trying to cast spell: " + spell.name);
		if(!spell.CastSpell(mousePosition)) {
			Debug.LogWarning("Casting spell failed...");
			return;
		}
		spellCastEvent?.Invoke(this, new SpellCastArgs(slot, spell.castDelay, spell.cooldown));
		anim_.SetTrigger("castSpell");
		anim_.SetBool("casting", true);
	}
	
	//public void CastSpell(VectorTarget target){
	//	spellSlots[0].CastSpell(new SpellParameters(target));
	//	anim_.SetTrigger("castSpell");
	//	anim_.SetBool("casting", true);
	//}
	
	//public void CastSpell(int slot, VectorTarget target){
	//	spellSlots[slot].CastSpell(new SpellParameters(target));
	//	anim_.SetTrigger("castSpell");
	//	anim_.SetBool("casting", true);
	//}

    //public void CastSpell(int slot, PointTarget target)
    //{
    //    spellSlots[slot].CastSpell(new SpellParameters(target));
    //    anim_.SetTrigger("castSpell");
    //    anim_.SetBool("casting", true);
    //}
}
