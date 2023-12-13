using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpellPanel : MonoBehaviour
{
	Dictionary<int, UISpellIcon> iconDict;
	
	void Awake(){
		var player = GameObject.Find("Player");
		player.GetComponentInChildren<Spellbook>().spellPrepared += OnSpellPrepared;
		player.GetComponent<Spellcaster>().spellCastEvent += OnSpellCast;
		iconDict = new	Dictionary<int, UISpellIcon>();
		foreach(UISpellIcon script in GetComponentsInChildren<UISpellIcon>()){
			iconDict[script.Id] = script;
		}
	}
	
	
	public void OnSpellPrepared(object? sender, SpellPreparedArgs args){
		UpdateIcon(args.Slot, args.Spell.icon);
	} 
   
	private void UpdateIcon(int slot, Sprite icon){
		iconDict[slot].ChangeIcon(icon);
	}
	
	private void OnSpellCast(object? sender, SpellCastArgs args){
		iconDict[args.Slot].StartDelayIndicator(args.CastDelay);
		StartCoroutine(OnSpellCooldown(args.CastDelay, args.Cooldown));
	}
	
	private IEnumerator OnSpellCooldown(float delay, float cooldown){
		yield return new WaitForSeconds(delay);
		Debug.Log("UI spell process cooldown");
	}
	
}
