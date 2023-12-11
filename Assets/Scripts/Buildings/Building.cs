using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IPlaceable
{
	private Collider2D placeCollider;
	private bool canBePlaced = true;
	private List<Collider2D> collisions;
	private Material[] outlineMats;
	private SpriteMask[] spriteMasks;
	private Collider2D[] childColliders;
	private bool isPlaced = false;
	public int attached = 0;
	public int detached = 0;
	void Awake(){
		if(isPlaced) return;

		childColliders = GetComponentsInChildren<Collider2D>();
		spriteMasks = GetComponentsInChildren<SpriteMask>();
		placeCollider = GetComponent<Collider2D>();
		collisions = new List<Collider2D>();
		var renderers = GetComponentsInChildren<SpriteRenderer>();
		outlineMats = new Material[renderers.Length];
		int i = 0;
		foreach (var item in renderers)
		{
			outlineMats[i] = item.material;
			i++;
		}
	}

	public bool TryPlaceBuilding(){
		if(!canBePlaced || isPlaced) return false;
		GetComponent<Cursor>().enabled = false;
		for(int i = 0; i < outlineMats.Length; i++){
			outlineMats[i].SetInt("_OutlineOn", 0);
			outlineMats[i].SetFloat("_OutlineThickness", 0);
			spriteMasks[i].enabled = false;
		}
		foreach (var collider in childColliders)
		{
			collider.enabled = true;
		}
		transform.SetParent(GameObject.Find("PlayerBuildings").transform);
		isPlaced = true;
		return true;
	}

	void OnTriggerEnter2D(Collider2D collider){
		try{
			if(collisions.Contains(collider)) return;
			collisions.Add(collider);
			attached++;
		
			if(canBePlaced){
				canBePlaced = false;
				foreach (var mat in outlineMats)
				{
					mat.SetInt("_Blocked", 1);
				}
			}
		}
		catch{
			Debug.LogError("Error removing collider!");
		}
		
	}

	void OnTriggerExit2D(Collider2D collider){
		try{
			if(!collisions.Contains(collider)) return;
			collisions.Remove(collider);
			detached++;
		
			if(collisions.Count == 0){
				canBePlaced = true;
				foreach (var mat in outlineMats)
				{
					mat.SetInt("_Blocked", 0);
				}
			}
		}
		catch{
			Debug.LogError("Error removing collider!");
		}
	}
}
