using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellcaster : MonoBehaviour
{
	public Spellbook spellbook;
	public int maxSpells;
	public ISpell[] spellSlots;
	Animator anim_;
    
	// Start is called before the first frame update
    void Start()
	{
		anim_ = GetComponentInChildren<Animator>();
		spellbook = GetComponentInChildren<Spellbook>();
		spellSlots = new ISpell[5];
		Debug.Log(spellbook.spellCount.ToString());
		for(int i = 0; i < maxSpells; i++){
			spellSlots[i] = spellbook.GetSpellById(i);
			if(spellSlots[i] != null)
				Debug.Log(spellSlots[i].name);
		}
	    ReadySpell(0, 0);
    }
    
	public void ReadySpell(int slot, int id){
		spellSlots[slot] = spellbook.GetSpellById(id);
	}

	public void AddSpell(Spell spell){
		GameObject newspell = new GameObject(spell.name);
		newspell.transform.SetParent(spellbook.transform);
		if(spell.projectile){
			var proj = newspell.AddComponent<Spawn_Projectile>();
		}
	}
	
	public void CastSpell(int slot, Vector3 mousePosition){
		Debug.Log("CastSpell slot " + slot.ToString());
		if(spellSlots[slot] == null){
			Debug.LogWarning("Player tried to cast spell at slot " + slot.ToString() + ", but does not have spell in that slot!");
			return;
		}
		spellSlots[slot].CastSpell(mousePosition);
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
