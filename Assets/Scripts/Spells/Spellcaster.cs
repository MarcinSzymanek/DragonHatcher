using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellcaster : MonoBehaviour
{
	public Spellbook spellbook;
	public int maxSpells;
	public SpellLogic[] spellSlots;
	Animator anim_;
	AudioSource audio_;
    
	// Start is called before the first frame update
    void Start()
	{
		anim_ = GetComponentInChildren<Animator>();
		audio_ = transform.Find("Spellbook").GetComponent<AudioSource>();
	    spellbook = GetComponentInChildren<Spellbook>();
	    spellSlots = new SpellLogic[maxSpells];
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
	
	public void CastSpell(){
		CastSpell(0);
	}
	
	public void CastSpell(int slot){
		var spell = spellSlots[slot];
		audio_.PlayOneShot(spell.GetAudioClip());
		spell.CastSpell();
		anim_.SetTrigger("castSpell");
		anim_.SetBool("casting", true);
	}
}
