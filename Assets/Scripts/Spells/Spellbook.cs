using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{
	private Dictionary<int, SpellLogic> heldSpells;
    // Start is called before the first frame update
	void Awake()
    {
	    heldSpells = new Dictionary<int, SpellLogic>();
	    var spells = GetComponentsInChildren<SpellLogic>();
	    foreach (SpellLogic s in spells){
	    	heldSpells[s.id] = s;
	    } 
    }

	public SpellLogic? GetSpellById(int id){
		if(heldSpells.ContainsKey(id)){	
			return heldSpells[id];
		}
		return null;
	}
}
