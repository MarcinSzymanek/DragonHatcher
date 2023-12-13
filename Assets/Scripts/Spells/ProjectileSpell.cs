using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ISpell<Vector3> means you must put Vector3 as param 
// SpellBase<VectorTarget> means that the spell effect needs to make a VectorTarget to cast it
public class ProjectileSpell : SpellBase<VectorTarget>, IVectorTargeted
{
	Spawn_Projectile projectileSpawner_;
	
	void Awake(){
		projectileSpawner_ = GetComponent<Spawn_Projectile>();
	}
	
	internal override VectorTarget getTarget(Vector3 mousePos){
		Vector3 parentPos = transform.parent.parent.position;
		return new VectorTarget(parentPos, Math2d.CalcDirection(parentPos, mousePos));
	}

	internal override void onCast(VectorTarget target){
		projectileSpawner_.Shoot(target);
	}
}
