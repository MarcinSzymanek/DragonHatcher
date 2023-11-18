using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{
	private Dictionary<int, ISpell> heldSpells;
	public int spellCount{get => heldSpells.Count;}
    // Start is called before the first frame update
	void Awake()
    {
	    heldSpells = new Dictionary<int, ISpell>();
	    var spells = GetComponentsInChildren<ISpell>();
	    foreach (ISpell s in spells){
	    	heldSpells[s.id] = s;
	    } 
    }

	public ISpell? GetSpellById(int id){
		if(heldSpells.ContainsKey(id)){
			return heldSpells[id];
		}
		return null;
	}
}
