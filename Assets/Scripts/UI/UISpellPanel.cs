using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpellPanel : MonoBehaviour
{
	Dictionary<int, UISpellIcon> iconDict;
	public List<UISpellIcon> debugList;
	private GameObject player;
	void Awake(){
		player = GameObject.Find("Player");
		player.GetComponentInChildren<Spellbook>().spellPrepared += OnSpellPrepared;
		player.GetComponent<Spellcaster>().spellCastEvent += OnSpellCast;
		iconDict = new	Dictionary<int, UISpellIcon>();
		foreach(UISpellIcon script in GetComponentsInChildren<UISpellIcon>()){
			iconDict[script.Id] = script;
			debugList.Add(script);
		}
	}
	
	void Start(){
		try{
			var list = player.GetComponentInChildren<Spellbook>().GetHeldSpellDataList();
			foreach (var item in list)
			{
				UpdateIcon(item.slot, item.icon);
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError("Something went wrong..." + e.Message);
		}
	}
	
	public void OnSpellPrepared(object? sender, SpellPreparedArgs args){
		UpdateIcon(args.Slot, args.Spell.icon);
	} 
   
	private void UpdateIcon(int slot, Sprite icon){
		iconDict[slot].ChangeIcon(icon);
	}
	
	private void OnSpellCast(object? sender, SpellCastArgs args){
		// Debug.Log("Spell has been cast!");
		iconDict[args.Slot].StartDelayIndicator(args.CastDelay);
		StartCoroutine(OnSpellCooldown(args.Slot, args.CastDelay, args.Cooldown));
	}
	
	private IEnumerator OnSpellCooldown(int slot, float delay, float cooldown){
		yield return new WaitForSeconds(delay);
		Debug.Log("UI spell process cooldown");
		iconDict[slot].StartCooldownIndicator(cooldown);
	}
	
	// This function is called when the MonoBehaviour will be destroyed.
	protected void OnDestroy()
	{
		var player = GameObject.Find("Player");
		try{
			player.GetComponentInChildren<Spellbook>().spellPrepared -= OnSpellPrepared;
			player.GetComponent<Spellcaster>().spellCastEvent -= OnSpellCast;	
		}
		catch (System.Exception e)
		{
			Debug.LogWarning("Well, we tried cleaning up, but got exception: " + e.Message);
		}
	}
	
}
