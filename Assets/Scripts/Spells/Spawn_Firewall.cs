using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Firewall : SpellBase<PointTarget>
{
    public GameObject firewallPrefab;

	internal override PointTarget getTarget(Vector3 mousePos){
		return new PointTarget(mousePos.x, mousePos.y);
	}

	internal override void onCast(PointTarget target){
		SpawnFirewall(target);
	}
    
    private void SpawnFirewall(PointTarget pt)
    {
        Vector3 position = new Vector3(pt.x, pt.y, 0);
        Instantiate(firewallPrefab, position, Quaternion.identity);
    }
}
