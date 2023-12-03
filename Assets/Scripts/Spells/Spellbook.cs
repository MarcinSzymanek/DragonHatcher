using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellPreparedArgs: EventArgs{
	public SpellPreparedArgs(int slot, SpellDataObject spell){
		Slot = slot;
		Spell = spell;
	}
	
	public int Slot{get;}
	public SpellDataObject Spell{get;}
}

public class Spellbook : MonoBehaviour
{
	private Dictionary<int, ISpell> heldSpells;
	public int spellCount{get => heldSpells.Count;}
	public List<SpellDataObject> knownSpells;
	
	public event EventHandler<SpellPreparedArgs> spellPrepared;
	
    // Start is called before the first frame update
	void Awake()
	{
		knownSpells = new List<SpellDataObject>();
	    heldSpells = new Dictionary<int, ISpell>();
	    updateHeldSpells_();
    }
	#nullable enable
	public ISpell? GetSpellById(int id){
		if(heldSpells.ContainsKey(id)){
			return heldSpells[id];
		}
		return null;
	}
	#nullable disable
	
	private void updateHeldSpells_(){
		var spells = GetComponentsInChildren<ISpell>();
		foreach (ISpell s in spells){
			Debug.Log("Adding " + s.name + " to index " + s.id.ToString());
			heldSpells[s.id] = s;
		} 
	}
	
	public void LearnSpell(SpellDataObject spell){
		if(knownSpells.Contains(spell)) {
			Debug.LogWarning(name + " tried to learn spell: " + spell.name + " but he already knows this spell.");
		}
		knownSpells.Add(spell);
		if(spellCount >= 5) return;
		PrepareSpell(spellCount, spell);
		
	}
	
	public void PrepareSpell(int slot, SpellDataObject spell){
		// update heldSpells with the new spell
		GameObject obj = Instantiate(spell.spellPrefab, transform);
		ISpell spellLogic = obj.GetComponent<ISpell>();
		spellLogic.id = slot;
		heldSpells[slot] = spellLogic; 
		spellPrepared?.Invoke(this, new SpellPreparedArgs(slot, spell));
	}
}
