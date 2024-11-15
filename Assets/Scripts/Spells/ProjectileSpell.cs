using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spawn_Projectile))]
public class ProjectileSpell : SpellBase, IVectorTargeted
{
	Spawn_Projectile projectileSpawner_;
	Transform parentTf_;
	
	void Awake(){
		projectileSpawner_ = GetComponent<Spawn_Projectile>();
		parentTf_ = transform.parent;
	}
	
	private VectorTarget getMouseVector()
	{
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return new VectorTarget(parentTf_, Math2d.CalcDirection(parentTf_.position, mousePos));
	}

	internal override void onCast(){	
		projectileSpawner_.Shoot(getMouseVector());
	}
}
