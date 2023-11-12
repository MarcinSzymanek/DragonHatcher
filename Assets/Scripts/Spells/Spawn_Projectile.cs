using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Projectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
	public LayerMask targetLayer;

    public float speed = 10f;

	public void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - firePoint.position).normalized;
		
	    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
	    setPrefabTarget(projectile);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * speed;

	    float angle = Math2d.GetDegreeFromVector(shootDirection, 90f);
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }
    
	void setPrefabTarget(GameObject obj){
		var col = obj.transform.GetChild(1).GetComponent<Collider2D>();
		col.excludeLayers += targetLayer;
		col = obj.transform.GetChild(0).GetComponent<Collider2D>();
		col.excludeLayers += targetLayer;
		var rb = obj.GetComponent<Rigidbody2D>();
		rb.excludeLayers += targetLayer;
	}
}