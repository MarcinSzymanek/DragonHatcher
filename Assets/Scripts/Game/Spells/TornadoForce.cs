using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class TornadoForce : MonoBehaviour
{
	public float time = 0.05f;
	public int knockbackForce = 0;
    
	void Awake(){
		GetComponent<ContactDamage>().damageEffectEvent += ApplyForce;
	}
    
    //private void OnCollisionEnter2D(Collision2D collider)
    //{
    //    Debug.Log("are we colliding?");
    //    Rigidbody2D enemyRigidbody = collider.rigidbody;
    //    if (enemyRigidbody != null)
    //    {
        	
    //    }

        
    //}
	
	public void ApplyForce(Rigidbody2D body){
		Debug.Log("Applying force");
		var move = body.GetComponent<Movement>();
		move.Stop();
		move.LockMovementFor(0.5f);
		Vector3 knockbackDirection = (body.transform.position - transform.position).normalized;
		body.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
	}
	
}
