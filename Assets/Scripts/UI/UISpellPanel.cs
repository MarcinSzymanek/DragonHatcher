using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpellPanel : MonoBehaviour
{
	Dictionary<int, UISpellIcon> iconDict;
	
	void Awake(){
		Spellbook book = GameObject.Find("Player").GetComponentInChildren<Spellbook>();
		book.spellPrepared += OnSpellPrepared;
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
}
