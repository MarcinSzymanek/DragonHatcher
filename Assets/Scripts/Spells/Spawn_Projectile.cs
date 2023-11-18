using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Projectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
	public LayerMask targetLayer;
	public LayerMask ownerLayer;
	
	public float rotationOffset;
	
	// How many times must we bitshift to set the layer: integer value of the layer
	private int layerInt_ = 0;

    public float speed = 10f;

	void Start(){
		int layerVal = ownerLayer.value;
		while(layerVal > 1){
			layerVal /= 2;
			layerInt_ ++;
		}
		Debug.Log("Layer int is: " + layerInt_.ToString());
	}

	public void Shoot(VectorTarget target)
	{
		
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
		
	    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
	    setPrefabTarget(projectile);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
		rb.velocity = target.direction * speed;

		//float angle = Math2d.GetDegreeFromVector(shootDirection, rotationOffset);
		projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, target.angle));

    }
    
	void setPrefabTarget(GameObject obj){
		obj.layer = layerInt_;
		var col = obj.transform.GetChild(1).GetComponent<Collider2D>();
		col.gameObject.layer = layerInt_;
	
		col = obj.transform.GetChild(0).GetComponent<Collider2D>();
		col.callbackLayers += targetLayer;
		col.contactCaptureLayers += targetLayer;
		col.gameObject.layer = layerInt_;
		var rb = obj.GetComponent<Rigidbody2D>();
	}
}