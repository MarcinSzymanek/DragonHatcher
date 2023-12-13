using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class TornadoForce : MonoBehaviour
{
    public float time = 0.1f;
    public int knockbackForce = 0;
    private void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log("are we colliding?");
        Rigidbody2D enemyRigidbody = collider.rigidbody;
        if (enemyRigidbody != null)
        {
            Vector3 knockbackDirection = (collider.transform.position - transform.position).normalized;
            enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        Invoke("destroy", time);
    }

    private void destroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
