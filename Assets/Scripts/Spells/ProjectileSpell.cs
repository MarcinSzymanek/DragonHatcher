using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpell : MonoBehaviour, ISpell, IVectorTargeted
{
	VectorTarget target_;
	public float castDelay;
	public int id{get; set;}
	string name_;
	public string name{get => name_;}
	Spawn_Projectile projectileSpawner_;
	
	void Awake(){
		name_ = gameObject.name;
		projectileSpawner_ = GetComponent<Spawn_Projectile>();
	}
	public void CastSpell(SpellParameters parameters){
		if(parameters.vectorTarget == null){
			Debug.LogError("Projectile spell cannot be cast with no parameters, " + gameObject.name);
			return;
		}
		castSpell_(parameters.vectorTarget);
	}
	
	private void castSpell_(VectorTarget target){
		target_ = target;
		Invoke("cast_", castDelay);
	}
	
	private void cast_(){
		projectileSpawner_.Shoot(target_);
	} 
}
