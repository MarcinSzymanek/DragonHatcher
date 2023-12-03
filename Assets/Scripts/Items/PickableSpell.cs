using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSpell : MonoBehaviour, IPickable
{
	public SpellDataObject spellData;
	private Spellbook playerSpellbook_;
	
	void Awake(){
		GetComponent<SpriteRenderer>().sprite = spellData.icon;
		playerSpellbook_ = GameObject.Find("Player").GetComponentInChildren<Spellbook>();
	}
	
	public void OnPickup(){
		Debug.Log("Player picked up spell: " + spellData.name);
		playerSpellbook_.LearnSpell(spellData);
		gameObject.SetActive(false);
	}
}
