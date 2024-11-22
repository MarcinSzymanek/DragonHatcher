using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Projectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

	public float rotationOffset;

	// How many times must we bitshift to set the layer: integer value of the layer
	private int layerInt_ = 0;

    public float speed = 10f;

	void Start(){
		if(firePoint == null) firePoint = transform.root;
	}


	public void Shoot(VectorTarget target, LinearVelocityData? velocityData = null)
	{
	    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
	    //setPrefabTarget(projectile);
		Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        
		if(velocityData != null)
		{
			projectile.GetComponent<LinearAcceleration>().Initialize(
				target.direction,
				velocityData.Value.StartSpeed,
				velocityData.Value.EndSpeed,
				velocityData.Value.SpeedDuration,
				velocityData.Value.LinearVelocityDelay
			);
		}
		else
		{
			rb.linearVelocity = target.direction * speed;	
		}

		projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, target.angle + rotationOffset));

    }

}