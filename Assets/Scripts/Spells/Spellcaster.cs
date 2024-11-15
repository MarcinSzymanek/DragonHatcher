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
	public static int id = 0;
	public Spellbook spellbook;
	public int maxSpells;
	public ISpell[] spellSlots;
	
	// The animator that is going to play casting animation
	public Animator Anim;
	
	public int debugSpellCount = 0;
    
	public event EventHandler<SpellCastArgs> spellCastEvent;
    
	// Start is called before the first frame update
    void Start()
	{
		id++;
		spellbook = GetComponentInChildren<Spellbook>();
		spellSlots = new ISpell[5];
		for(int i = 0; i < maxSpells; i++){
			spellSlots[i] = spellbook.GetSpellById(i);
			if(spellSlots[i] != null) debugSpellCount++;
		}
	    ReadySpell(0, 0);
    }
    
	public void ReadySpell(int slot, int id){
		
		spellSlots[slot] = spellbook.GetSpellById(id);
		int i = 0;
		foreach(var s in spellSlots)
		{
			if(s is not null){
				i++;
			}
		}
		debugSpellCount = i;
	}
	
	public void CastSpell(int slot){

		ISpell? spell = spellbook.GetSpellById(slot);
		if(spell is null){
			Debug.LogWarning("Player tried to cast spell at slot " + slot.ToString() + ", but does not have spell in that slot!");
			return;
		}
		if(!spell.CastSpell()) {
			return;
		}
		spellCastEvent?.Invoke(this, new SpellCastArgs(slot, spell.castDelay, spell.cooldown));
		Anim.SetTrigger("Cast");
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
